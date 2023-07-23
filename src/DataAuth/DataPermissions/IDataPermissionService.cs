using DataAuth.Entities;
using DataAuth.Enums;

namespace DataAuth.DataPermissions
{
    public interface IDataPermissionService
    {
        Task AddDataPermission(DataPermission model, CancellationToken cancellationToken = default);
        Task UpdateDataPermission(
            DataPermission model,
            CancellationToken cancellationToken = default
        );

        Task DeleteDataPermission(int id, CancellationToken cancellationToken = default);

        Task<DataPermissionModel?> GetDataPermissionById(
            int id,
            CancellationToken cancellationToken = default
        );

        Task<IEnumerable<DataPermissionModel>> GetDataPermissions(
            GrantType? grantType = null,
            string? subjectId = null,
            int? accessAttributeTableId = null,
            CancellationToken cancellationToken = default
        );
    }
}
