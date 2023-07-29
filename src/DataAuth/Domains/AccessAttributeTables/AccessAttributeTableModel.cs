using DataAuth.Base;

namespace DataAuth.Domains.AccessAttributeTables
{
    public class AccessAttributeTableModel : BaseModel<int>
    {
        public AccessAttributeTableModel(
            int accessAttributeId,
            string tableName,
            string alias,
            string idColumn,
            string nameColumn,
            bool isSelfReference,
            string? parentColumn,
            int? hierarchyLevel,
            bool isLeafLevel,
            string? localPermissionTableName,
            string? localPermissionIdColumn,
            string? localPermissionLookupColumn
        )
        {
            AccessAttributeId = accessAttributeId;
            TableName = tableName;
            Alias = alias;
            IdColumn = idColumn;
            NameColumn = nameColumn;
            IsSelfReference = isSelfReference;
            ParentColumn = parentColumn;
            HierarchyLevel = hierarchyLevel;
            IsLeafLevel = isLeafLevel;
            LocalPermissionTableName = localPermissionTableName;
            LocalPermissionIdColumn = localPermissionIdColumn;
            LocalPermissionLookupColumn = localPermissionLookupColumn;
        }

        public int AccessAttributeId { get; set; }

        /// <summary>
        /// The database table name of the data that is used as filter for data.
        /// </summary>
        public string TableName { get; set; }

        public string Alias { get; set; }

        public string IdColumn { get; set; }

        public string NameColumn { get; set; }

        public string? ParentColumn { get; set; }

        public bool IsSelfReference { get; set; }

        public int? HierarchyLevel { get; set; }

        public bool IsLeafLevel { get; set; }

        public string? LocalPermissionTableName { get; set; }

        public string? LocalPermissionIdColumn { get; set; }

        public string? LocalPermissionLookupColumn { get; set; }
    }
}
