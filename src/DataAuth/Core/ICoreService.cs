using DataAuth.Entities;
using DataAuth.Enums;
using Microsoft.Data.SqlClient;

namespace DataAuth.Core
{
    public interface ICoreService
    {
        Task<DataPermissionResult<TKey>> GetDataPermissions<TKey>(
            string subjectId,
            string accessAttributeCode,
            GrantType grantType = GrantType.ForUser,
            string? localLookupValue = null,
            string functionCode = FunctionCode.All,
            CancellationToken cancellationToken = default
        );
        Task<(string QueryString, IList<SqlParameter> QueryParams)> GenerateQueryString(
            DataPermission permission,
            string? localLookupValue = null
        );

        Task<List<DataPermission>> GetDataPermissionEntities(
            string subjectId,
            string accessAttributeCode,
            GrantType grantType,
            string functionCode,
            CancellationToken cancellationToken
        );
    }
}
