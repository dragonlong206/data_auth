using DataAuth.Cache;
using DataAuth.Entities;
using DataAuth.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataAuth.Core
{
    public class CoreService : ICoreService
    {
        DataAuthDbContext _dbContext;
        ICacheProvider _cacheProvider;

        public CoreService(DataAuthDbContext dbContext, ICacheProvider cacheProvider)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
        }

        public static string GetCacheKey(
            string subjectId,
            string accessAttributeCode,
            GrantType grantType
        )
        {
            return string.Join("_", subjectId, accessAttributeCode, grantType);
        }

        public async Task<DataPermissionResult<TKey>> GetDataPermissions<TKey>(
            string subjectId,
            string accessAttributeCode,
            GrantType grantType = GrantType.ForUser,
            string? localLookupValue = null,
            CancellationToken cancellationToken = default
        )
            where TKey : struct
        {
            var result = new DataPermissionResult<TKey>();
            var cacheKey = GetCacheKey(subjectId, accessAttributeCode, grantType);
            var dataFromCache = _cacheProvider.Get<DataPermissionResult<TKey>>(cacheKey);
            if (dataFromCache != null)
            {
                return dataFromCache;
            }

            await GetDataPermissionFromDatabase(
                subjectId,
                accessAttributeCode,
                grantType,
                result,
                cancellationToken
            );

            _cacheProvider.Set(cacheKey, result);

            return result;
        }

        private async Task GetDataPermissionFromDatabase<TKey>(
            string subjectId,
            string accessAttributeCode,
            GrantType grantType,
            DataPermissionResult<TKey> result,
            CancellationToken cancellationToken
        )
            where TKey : struct
        {
            var dataPermissions = await _dbContext.DataPermissions
                .AsNoTracking()
                .Include(x => x.AccessAttributeTable)
                .ThenInclude(a => a!.AccessAttribute)
                .Where(
                    x =>
                        x.SubjectId == subjectId
                        && x.GrantType == grantType
                        && x.AccessAttributeTable!.AccessAttribute!.Code == accessAttributeCode
                )
                .ToListAsync(cancellationToken);

            // If subject is user then get all roles of the user and then get all permissions of those roles.
            if (grantType == GrantType.ForUser)
            {
                var dataPermissionOfRoles = await GetPermissionsByRolesOfUser(
                    subjectId,
                    accessAttributeCode,
                    cancellationToken
                );
                dataPermissions.AddRange(dataPermissionOfRoles);
            }
            var allGrantedData = new List<TKey>();
            if (dataPermissions != null && dataPermissions.Any())
            {
                foreach (var permission in dataPermissions)
                {
                    var query = await GenerateQueryString(permission);
                    var grantedData = await _dbContext.Database
                        .SqlQueryRaw<TKey>(query.QueryString, query.QueryParams.ToArray())
                        .ToListAsync(cancellationToken);
                    var resultDetail = new DataPermissionResultDetail<TKey>()
                    {
                        AccessLevel = permission.AccessLevel,
                        IdColumn = permission.AccessAttributeTable!.IdColumn,
                        TableName = permission.AccessAttributeTable!.TableName,
                        GrantedValues = grantedData
                    };
                    allGrantedData.AddRange(grantedData);
                }
                result.GrantedValues = allGrantedData.Distinct().ToArray();
            }
        }

        private async Task<List<DataPermission>> GetPermissionsByRolesOfUser(
            string userId,
            string accessAttributeCode,
            CancellationToken cancellationToken
        )
        {
            var roleIdsOfUser = await _dbContext.UserRoles
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId.ToString())
                .ToListAsync(cancellationToken);
            if (roleIdsOfUser.Any())
            {
                var rolePermissions = await _dbContext.DataPermissions
                    .AsNoTracking()
                    .Include(x => x.AccessAttributeTable)
                    .ThenInclude(a => a!.AccessAttribute)
                    .Where(
                        x =>
                            roleIdsOfUser.Contains(x.SubjectId)
                            && (x.GrantType == GrantType.ForRole)
                            && x.AccessAttributeTable!.AccessAttribute!.Code == accessAttributeCode
                    )
                    .ToListAsync(cancellationToken);
                return rolePermissions;
            }
            return new List<DataPermission>();
        }

        public async Task<(
            string QueryString,
            IList<SqlParameter> QueryParams
        )> GenerateQueryString(DataPermission permission, string? localLookupValue = null)
        {
            var attributeTable = permission.AccessAttributeTable!;
            var query = string.Empty;
            var queryParams = new List<SqlParameter>();
            switch (permission.AccessLevel)
            {
                case AccessLevel.Local:
                    // With local permission, look up to local permission table to get local permission value
                    query =
                        CreateSelectQuery(attributeTable)
                        + $"\nWHERE {attributeTable.Alias}.[{attributeTable.IdColumn}] IN "
                        + $"(SELECT [{attributeTable.LocalPermissionLookupColumn!}] FROM [{attributeTable.LocalPermissionTableName!}] WHERE [{attributeTable.LocalPermissionIdColumn!}] = @LookupValue)";
                    queryParams.Add(new SqlParameter("LookupValue", localLookupValue!));
                    break;
                case AccessLevel.Specific:
                    query =
                        CreateSelectQuery(attributeTable)
                        + $"\nWHERE {attributeTable.Alias}.[{attributeTable.IdColumn}] = @LookupValue";
                    queryParams.Add(new SqlParameter("LookupValue", permission.GrantedDataValue!));
                    break;
                case AccessLevel.Deep:
                    // With deep permission, there are 2 types of hierarchy
                    // Self reference hierarchy:
                    //   - With this type of hierarchy, we need to recursive loop through every level from selected node to leaf nodes.
                    // Separated-level hierarchy:
                    //   - With this type of hierarchy, we get all related levels, join together to get the leaf level data.
                    if (permission.AccessAttributeTable!.IsSelfReference)
                    {
                        (query, queryParams) = CreateQueryForSelfReferenceHierarchy(permission);
                    }
                    else
                    {
                        (query, queryParams) = await CreateQueryForLevelSeparatedHierarchy(
                            permission
                        );
                    }
                    break;
                case AccessLevel.Global:
                    query = CreateSelectQuery(attributeTable);
                    break;
            }

            return (query, queryParams);
        }

        private static string CreateSelectQuery(AccessAttributeTable tableMetadata)
        {
            return $"SELECT {tableMetadata.Alias}.[{tableMetadata.IdColumn}] FROM [{tableMetadata.TableName}] AS {tableMetadata.Alias}";
        }

        private static (
            string QueryString,
            List<SqlParameter> QueryParams
        ) CreateQueryForSelfReferenceHierarchy(DataPermission permission)
        {
            var attributeTable = permission.AccessAttributeTable!;
            // Recursive cte with max recursion level = 20 to ensure performance
            var query =
                $";WITH cte AS"
                + $"\n(SELECT [{attributeTable.IdColumn}], [{attributeTable.ParentColumn}] FROM [{attributeTable.TableName}] WHERE [{attributeTable.IdColumn}] = @LookupValue"
                + $"\nUNION ALL"
                + $"\nSELECT t2.[{attributeTable.IdColumn}], t2.[{attributeTable.ParentColumn}] FROM cte t1"
                + $"\nJOIN [{attributeTable.TableName}] t2 ON t1.[{attributeTable.IdColumn}] = t2.[{attributeTable.ParentColumn}])"
                + $"\nSELECT [{attributeTable.IdColumn}] FROM cte"
                + $"\nOPTION (MAXRECURSION 20)";

            var queryParams = new List<SqlParameter>
            {
                new SqlParameter("LookupValue", permission.GrantedDataValue)
            };

            return (query, queryParams);
        }

        /// <summary>
        /// Example:
        /// SELECT s.Id
        ///    FROM Regions r
        ///    JOIN Provinces p ON r.Id = p.RegionId
        ///    JOIN Stores s ON p.Id = s.ProvinceId
        ///    WHERE r.Id = 1;
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        private async Task<(
            string QueryString,
            List<SqlParameter> QueryParams
        )> CreateQueryForLevelSeparatedHierarchy(DataPermission permission)
        {
            // Get all lower levels in hierarchy from current node
            var rootTable = permission.AccessAttributeTable!;
            string query;
            if (!rootTable.IsLeafLevel)
            {
                var lowerLevelTables = await (
                    _dbContext.AccessAttributeTables
                        .AsNoTracking()
                        .Where(
                            x =>
                                x.AccessAttributeId == rootTable.AccessAttributeId
                                && x.HierarchyLevel > rootTable.HierarchyLevel
                        )
                        .OrderBy(x => x.HierarchyLevel)
                        .ToListAsync()
                );
                var leafLevelTable = lowerLevelTables.FirstOrDefault(x => x.IsLeafLevel);
                if (leafLevelTable == null)
                {
                    throw new Exception("Missing leaf level table for this access attribute!");
                }
                query =
                    $"SELECT {leafLevelTable.Alias}.[{leafLevelTable.IdColumn}] FROM [{rootTable.TableName}] AS {rootTable.Alias}";
                query = CreateJoinQueryForHierarchy(query, rootTable, lowerLevelTables);
            }
            else
            {
                query = CreateSelectQuery(rootTable);
            }
            query += $"\nWHERE {rootTable.Alias}.[{rootTable.IdColumn}] = @LookupValue";

            var queryParams = new List<SqlParameter>
            {
                new SqlParameter("LookupValue", permission.GrantedDataValue!)
            };

            return (query, queryParams);
        }

        public static string CreateJoinQueryForHierarchy(
            string currentQuery,
            AccessAttributeTable rootTable,
            List<AccessAttributeTable> lowerLevelTables
        )
        {
            if (lowerLevelTables != null && lowerLevelTables.Any())
            {
                var previousTable = rootTable;
                foreach (var table in lowerLevelTables)
                {
                    currentQuery +=
                        $"\n\tJOIN [{table.TableName}] AS {table.Alias} ON {previousTable.Alias}.[{previousTable.IdColumn}] = {table.Alias}.[{table.ParentColumn}]";
                    previousTable = table;
                }
            }

            return currentQuery;
        }
    }
}
