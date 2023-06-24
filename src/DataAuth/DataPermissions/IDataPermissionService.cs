namespace DataAuth.DataPermissions
{
    public interface IDataPermissionService
    {
        Task<DataPermissionModel> AddDataPermission(DataPermissionModel model);
    }
}
