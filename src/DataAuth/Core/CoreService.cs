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

        public async Task<DataPermissionResult<TData>> GetDataPermissions<TData>(string subjectId, GrantType grantType, string accessAttributeCode, string? localLookupValue = null, CancellationToken cancellationToken = default) where TData : struct
        {
            var result = new DataPermissionResult<TData>();
            var dataPermissions = await _dbContext.DataPermissions
                .Include(x => x.AccessAttributeTable)
                .ThenInclude(a => a.AccessAttribute)
                .Where(x => x.SubjectId == subjectId && x.GrantType == grantType && x.AccessAttributeTable!.AccessAttribute!.Code == accessAttributeCode).ToListAsync(cancellationToken);
            var allGrantedData = new List<TData>();
            if (dataPermissions != null && dataPermissions.Any())
            {
                foreach (var permission in dataPermissions)
                {
                    var query = GenerateQueryString(permission);
                    var grantedData = await _dbContext.Database.SqlQueryRaw<TData>(query.QueryString, query.QueryParams.Select(x => x.Value).ToArray()).ToListAsync(cancellationToken);
                    var resultDetail = new DataPermissionResultDetail<TData>()
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

        public (string QueryString, IList<SqlParameter> QueryParams) GenerateQueryString(DataPermission permission, string? localLookupValue = null)
        {
            var tableMetadata = permission.AccessAttributeTable!;
            var query = $"SELECT {tableMetadata.Alias}.[{tableMetadata.IdColumn}] FROM [{tableMetadata.TableName}] AS {tableMetadata.Alias}";
            var queryParams = new List<SqlParameter>();
            switch (permission.AccessLevel)
            {
                case AccessLevel.Local:
                    // TODO: Validate required fields
                    query += "\n" +
                        $"WHERE {tableMetadata.Alias}.[{tableMetadata.IdColumn}] IN " +
                        $"(SELECT [{tableMetadata.LocalPermissionLookupColumn!}] FROM [{tableMetadata.LocalPermissionTableName!}] WHERE [{tableMetadata.LocalPermissionIdColumn!}] = @LookupValue)";
                    queryParams.Add(new SqlParameter("LookupValue", localLookupValue!));
                    break;
                case AccessLevel.Deep:
                    break;
                case AccessLevel.Global:
                    // Nothing more to do
                    break;
            }

            return (query, queryParams);
        }
    }
}
