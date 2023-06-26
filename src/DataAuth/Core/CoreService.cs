﻿using DataAuth.Entities;
using DataAuth.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            var attributeTable = permission.AccessAttributeTable!;
            var query = string.Empty;
            var queryParams = new List<SqlParameter>();
            switch (permission.AccessLevel)
            {
                case AccessLevel.Local:
                    // With local permission, look up to local permission table to get local permission value
                    query = CreateSelectQueryForAccessAttributeTable(attributeTable) +
                        $"\nWHERE {attributeTable.Alias}.[{attributeTable.IdColumn}] IN " +
                        $"(SELECT [{attributeTable.LocalPermissionLookupColumn!}] FROM [{attributeTable.LocalPermissionTableName!}] WHERE [{attributeTable.LocalPermissionIdColumn!}] = @LookupValue)";
                    queryParams.Add(new SqlParameter("LookupValue", localLookupValue!));
                    break;
                case AccessLevel.Specific:
                    query = CreateSelectQueryForAccessAttributeTable(attributeTable) +
                        $"\nWHERE {attributeTable.Alias}.[{attributeTable.IdColumn}] = @LookupValue";
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
                        (query, queryParams) = await CreateQueryForLevelSeparatedHierarchy(permission);
                    }
                    break;
                case AccessLevel.Global:
                    query = CreateSelectQueryForAccessAttributeTable(attributeTable);
                    break;
            }

            return (query, queryParams);
        }

        private static string CreateSelectQueryForAccessAttributeTable(AccessAttributeTable tableMetadata)
        {
            return $"SELECT {tableMetadata.Alias}.[{tableMetadata.IdColumn}] FROM [{tableMetadata.TableName}] AS {tableMetadata.Alias}";
        }

        private static (string QueryString, List<SqlParameter> QueryParams) CreateQueryForSelfReferenceHierarchy(DataPermission permission)
        {
            var attributeTable = permission.AccessAttributeTable!;
            // Recursive cte with max recursion level = 20 to ensure performance
            var query = $";WITH cte AS" +
                $"(SELECT [{attributeTable.IdColumn}], [{attributeTable.ParentColumn}] FROM [{attributeTable.TableName}] WHERE [{attributeTable.IdColumn}] = @LookupValue" +
                $"UNION ALL" +
                $"SELECT t2.[{attributeTable.IdColumn}], t2.[{attributeTable.ParentColumn}] FROM cte t1" +
                $"JOIN [{attributeTable.TableName}] t2 ON t1.[{attributeTable.IdColumn}] = t2.[{attributeTable.ParentColumn}])" +
                $"SELECT [{attributeTable.IdColumn}] FROM cte" +
                $"OPTION (MAXRECURSION 20)";

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
        private async Task<(string QueryString, List<SqlParameter> QueryParams)> CreateQueryForLevelSeparatedHierarchy(DataPermission permission)
        {
            // Get all lower levels in hierarchy from current node
            var rootTable = permission.AccessAttributeTable!;
            var query = CreateSelectQueryForAccessAttributeTable(rootTable);
            var lowerLevelTables = await (_dbContext.AccessAttributeTables.AsNoTracking()
                .Where(x => x.AccessAttributeId == rootTable.AccessAttributeId
                    && x.HierarchyLevel > rootTable.HierarchyLevel)
                .OrderBy(x => x.HierarchyLevel)
                .ToListAsync());
            query = CreateJoinQueryForHierarchy(query, rootTable, lowerLevelTables);
            query += $"\nWHERE {rootTable.Alias}.[{rootTable.IdColumn}] = @LookupValue";
            var queryParams = new List<SqlParameter>
            {
                new SqlParameter("LookupValue", permission.GrantedDataValue!)
            };

            return (query, queryParams);
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
