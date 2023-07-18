using DataAuth.Enums;

namespace DataAuth.DataPermissions
{
    public interface IDataPermissionService
    {
        Task<DataPermissionModel> AddDataPermission(
            DataPermissionModel model,
            CancellationToken cancellationToken = default
        );
        Task<DataPermissionModel> UpdateDataPermission(
            DataPermissionModel model,
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
