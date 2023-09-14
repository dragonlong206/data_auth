using DataAuth.Enums;

namespace DataAuth.Core
{
    public class DataPermissionResult<TKey>
    {
        public DataPermissionResult()
        {
            PermissionDetails = new List<DataPermissionResultDetail<TKey>>();
            GrantedValues = new List<TKey>();
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

        public IEnumerable<DataPermissionResultDetail<TKey>> PermissionDetails { get; set; }
    }

    public class DataPermissionResultDetail<TKey>
    {
        public int Id { get; set; }

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
