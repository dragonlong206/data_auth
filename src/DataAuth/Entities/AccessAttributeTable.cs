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

        public AccessAttributeTable(int accessAttributeId, string tableName, string alias, string idColumn, string nameColumn, bool isSelfReference
            , string? parentColumn, int? hierarchyLevel, bool isLeafLevel = false, string? localPermissionTableName = null, string? localPermissionIdColumn = null, string? localPermissionLookupColumn = null)
            : this(accessAttributeId, tableName, alias, idColumn, nameColumn)
        {
            IsSelfReference = isSelfReference;
            ParentColumn = parentColumn;
            HierarchyLevel = hierarchyLevel;
            IsLeafLevel = isLeafLevel;
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

        /// <summary>
        /// A table is self reference if the foreign key point to the table itself.
        /// </summary>
        public bool IsSelfReference { get; set; }

        /// <summary>
        /// Hierarchy level for hierarchy data. Hierarchy level starts from 0 for root node and increases 1 for each level in the hierarchy.
        /// </summary>
        public int? HierarchyLevel { get; set; }

        /// <summary>
        /// One access attribute has only one leaf level table.
        /// </summary>
        public bool IsLeafLevel { get; set; }

        public string? LocalPermissionTableName { get; set; }

        public string? LocalPermissionIdColumn { get; set; }

        public string? LocalPermissionLookupColumn { get; set; }

    }
}
