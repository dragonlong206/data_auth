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
        )
            where TKey : struct;
        Task<(string QueryString, IList<SqlParameter> QueryParams)> GenerateQueryString(
            DataPermission permission,
            string? localLookupValue = null
        );
    }
}
