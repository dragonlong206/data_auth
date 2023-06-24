using DataAuth.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Entities
{
    public class AccessAttributeTable : BaseEntity<int>
    {
        public AccessAttributeTable(int accessAttributeId, string tableName, string alias, string idColumn, string nameColumn)
        {
            AccessAttributeId = accessAttributeId;
            TableName = tableName;
            Alias = alias;
            IdColumn = idColumn;
            NameColumn = nameColumn;
        }

        public AccessAttributeTable(int accessAttributeId, string tableName, string alias, string idColumn, string nameColumn
            , string? parentColumn, int hierarchyLevel, string? localPermissionTableName, string? localPermissionIdColumn, string? localPermissionLookupColumn)
            : this(accessAttributeId, tableName, alias, idColumn, nameColumn)
        {
            ParentColumn = parentColumn;
            HierarchyLevel = hierarchyLevel;
            LocalPermissionTableName = localPermissionTableName;
            LocalPermissionIdColumn = localPermissionIdColumn;
            LocalPermissionLookupColumn = localPermissionLookupColumn;
        }

        public int AccessAttributeId { get; set; }

        public AccessAttribute? AccessAttribute { get; set; }

        /// <summary>
        /// The database table name of the data that is used as filter for data.
        /// </summary>
        public string TableName { get; set; }

        public string Alias { get; set; }

        public string IdColumn { get; set; }

        public string NameColumn { get; set; }

        public string? ParentColumn { get; set; }

        public int HierarchyLevel { get; set; }

        public string? LocalPermissionTableName { get; set; }

        public string? LocalPermissionIdColumn { get; set; }

        public string? LocalPermissionLookupColumn { get; set; }

    }
}
