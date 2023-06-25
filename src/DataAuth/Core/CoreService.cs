using DataAuth.Entities;
using DataAuth.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataAuth.Core
{
    public class CoreService : ICoreService
    {
        DataAuthDbContext _dbContext;

        public CoreService(DataAuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DataPermissionResult<TKey>> GetDataPermissions<TKey>(string subjectId, GrantType grantType, string accessAttributeCode, string? localLookupValue = null, CancellationToken cancellationToken = default) where TKey : struct
        {
            var result = new DataPermissionResult<TKey>();
            var dataPermissions = await _dbContext.DataPermissions.AsNoTracking()
                .Include(x => x.AccessAttributeTable)
                .ThenInclude(a => a.AccessAttribute)
                .Where(x => x.SubjectId == subjectId && x.GrantType == grantType && x.AccessAttributeTable!.AccessAttribute!.Code == accessAttributeCode).ToListAsync(cancellationToken);
            var allGrantedData = new List<TKey>();
            if (dataPermissions != null && dataPermissions.Any())
            {
                foreach (var permission in dataPermissions)
                {
                    var query = await GenerateQueryString(permission);
                    var grantedData = await _dbContext.Database.SqlQueryRaw<TKey>(query.QueryString, query.QueryParams.Select(x => x.Value).ToArray()).ToListAsync(cancellationToken);
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

            return result;
        }

        public async Task<(string QueryString, IList<SqlParameter> QueryParams)> GenerateQueryString(DataPermission permission, string? localLookupValue = null)
        {
            var tableMetadata = permission.AccessAttributeTable!;
            var query = CreateSelectQueryForAccessAttributeTable(tableMetadata);
            var queryParams = new List<SqlParameter>();
            switch (permission.AccessLevel)
            {
                case AccessLevel.Local:
                    // With local permission, look up to local permission table to get local permission value
                    query += "\n" +
                        $"WHERE {tableMetadata.Alias}.[{tableMetadata.IdColumn}] IN " +
                        $"(SELECT [{tableMetadata.LocalPermissionLookupColumn!}] FROM [{tableMetadata.LocalPermissionTableName!}] WHERE [{tableMetadata.LocalPermissionIdColumn!}] = @LookupValue)";
                    queryParams.Add(new SqlParameter("LookupValue", localLookupValue!));
                    break;
                case AccessLevel.Deep:
                    // With deep permission, there are 2 types of hierarchy
                    // Self reference hierarchy:
                    //   - With this type of hierarchy, we need to recursive loop through every level from selected node to leaf nodes.
                    // Separated-level hierarchy:
                    //   - With this type of hierarchy, we get all related levels, join together to get the leaf level data.
                    if (permission.AccessAttributeTable!.IsSelfReference)
                    {
                        query = CreateQueryForSelfReferenceHierarchy(permission);
                    }
                    else
                    {
                        /** Example query
                         SELECT s.Id
                            FROM Regions r
                            JOIN Provinces p ON r.Id = p.RegionId
                            JOIN Stores s ON p.Id = s.ProvinceId
                            WHERE r.Id = 1;
                         */
                        (query, queryParams) = await CreateQueryForLevelSeparatedHierarchy(permission, query);
                    }
                    break;
                case AccessLevel.Global:
                    // Nothing more to do
                    break;
            }

            return (query, queryParams);
        }

        private static string CreateSelectQueryForAccessAttributeTable(AccessAttributeTable tableMetadata)
        {
            return $"SELECT {tableMetadata.Alias}.[{tableMetadata.IdColumn}] FROM [{tableMetadata.TableName}] AS {tableMetadata.Alias}";
        }

        private static string CreateQueryForSelfReferenceHierarchy(DataPermission permission)
        {
            // Recursive cte with max recursion level = 20 to ensure performance
            var cte = $";WITH cte AS" +
                $"(SELECT * FROM dbo.table_name" +
                $"UNION ALL" +
                $"SELECT t2.* FROM cte t1" +
                $"JOIN dbo.table_name t2 ON t1.parent_column_name = t2.column_name)" +
                $"SELECT * FROM cte" +
                $"OPTION (MAXRECURSION 20)";

            return cte;
        }

        private async Task<(string QueryString, List<SqlParameter> QueryParams)> CreateQueryForLevelSeparatedHierarchy(DataPermission permission, string currentQuery)
        {
            // Get all lower levels in hierarchy from current node
            var rootTable = permission.AccessAttributeTable!;
            var lowerLevelTables = await (_dbContext.AccessAttributeTables.AsNoTracking()
                .Where(x => x.AccessAttributeId == rootTable.AccessAttributeId
                    && x.HierarchyLevel > rootTable.HierarchyLevel)
                .OrderBy(x => x.HierarchyLevel)
                .ToListAsync());
            currentQuery = CreateJoinQueryForHierarchy(currentQuery, rootTable, lowerLevelTables);
            currentQuery += $"\nWHERE {rootTable.Alias}.[{rootTable.IdColumn}] = @LookupValue";
            var queryParams = new List<SqlParameter>
            {
                new SqlParameter("LookupValue", permission.GrantedDataValue!)
            };

            return (currentQuery, queryParams);
        }

        public static string CreateJoinQueryForHierarchy(string currentQuery, AccessAttributeTable rootTable, List<AccessAttributeTable> lowerLevelTables)
        {
            if (lowerLevelTables != null && lowerLevelTables.Any())
            {
                var previousTable = rootTable;
                foreach (var table in lowerLevelTables)
                {
                    currentQuery += $"\n\tJOIN [{table.TableName}] AS {table.Alias} ON {previousTable.Alias}.[{previousTable.IdColumn}] = {table.Alias}.[{table.ParentColumn}]";
                    previousTable = table;
                }
            }

            return currentQuery;
        }
    }
}
