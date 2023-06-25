using DataAuth.Enums;

namespace DataAuth.Core
{
    public class DataPermissionResult<TKey> where TKey : struct
    {
        public DataPermissionResult()
        {
            PermissionDetails = new List<DataPermissionResultDetail<TKey>>();
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
        public IEnumerable<TKey>? GrantedValues { get; set; }

        IEnumerable<DataPermissionResultDetail<TKey>> PermissionDetails { get; set; }
    }

    public class DataPermissionResultDetail<TKey> where TKey: struct
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
        public IEnumerable<TKey>? GrantedValues { get; set; }
    }
}
