using DataAuth.Enums;

namespace DataAuth.Core
{
    public class DataPermissionResult<TData> where TData : struct
    {
        public DataPermissionResult()
        {
            PermissionDetails = new List<DataPermissionResultDetail<TData>>();
        }

        /// <summary>
        /// Name of database table containing granted data.
        /// </summary>
        public string? TableName { get; set; }

        /// <summary>
        /// Name of ID column in the database table containing granted data.
        /// </summary>
        public string? IdColumn { get; set; }

        /// <summary>
        /// The list of granted IDs
        /// </summary>
        public IEnumerable<TData>? GrantedValues { get; set; }

        IEnumerable<DataPermissionResultDetail<TData>> PermissionDetails { get; set; }
    }

    public class DataPermissionResultDetail<TData> where TData: struct
    {
        /// <summary>
        /// Name of database table containing granted data.
        /// </summary>
        public string? TableName { get; set; }

        /// <summary>
        /// Name of ID column in the database table containing granted data.
        /// </summary>
        public string? IdColumn { get; set; }

        public AccessLevel AccessLevel { get; set; }

        /// <summary>
        /// The list of granted IDs
        /// </summary>
        public IEnumerable<TData>? GrantedValues { get; set; }
    }
}
