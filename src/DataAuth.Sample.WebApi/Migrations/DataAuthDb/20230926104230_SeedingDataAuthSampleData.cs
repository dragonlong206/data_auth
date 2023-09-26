using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAuth.Sample.WebApi.Migrations.DataAuthDb
{
    /// <inheritdoc />
    public partial class SeedingDataAuthSampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "DataAuth",
                table: "AccessAttributes",
                columns: new[] { "Id", "Code", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "DEPT", "Data permission by department hierarchy", "Department" },
                    { 2, "STORE", "Data permission by store hierarchy", "Store" }
                });

            migrationBuilder.InsertData(
                schema: "DataAuth",
                table: "Roles",
                columns: new[] { "Id", "Code", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "IT_MANAGER", null, "IT Department Manager" },
                    { 2, "SALE_MANAGER", null, "Sales Manager" },
                    { 3, "NORTHERN_SALE_MANAGER", null, "Northern Sales Manager" }
                });

            migrationBuilder.InsertData(
                schema: "DataAuth",
                table: "AccessAttributeTables",
                columns: new[] { "Id", "AccessAttributeId", "Alias", "HierarchyLevel", "IdColumn", "IsLeafLevel", "IsSelfReference", "LocalPermissionIdColumn", "LocalPermissionLookupColumn", "LocalPermissionTableName", "NameColumn", "ParentColumn", "TableName" },
                values: new object[,]
                {
                    { 1, 1, "d", null, "Id", false, true, null, null, null, "Name", "ParentDepartmentId", "Departments" },
                    { 2, 2, "r", 0, "Id", false, false, null, null, null, "Name", null, "Regions" },
                    { 3, 2, "p", 1, "Id", false, false, null, null, null, "Name", "RegionId", "Provinces" },
                    { 4, 2, "s", 2, "Id", true, true, "Id", "LastWorkingStoreId", "Employees", "Name", "ProvinceId", "Stores" }
                });

            migrationBuilder.InsertData(
                schema: "DataAuth",
                table: "UserRoles",
                columns: new[] { "Id", "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "1" },
                    { 2, 2, "2" }
                });

            migrationBuilder.InsertData(
                schema: "DataAuth",
                table: "DataPermissions",
                columns: new[] { "Id", "AccessAttributeTableId", "AccessLevel", "FunctionCode", "GrantType", "GrantedDataValue", "SubjectId" },
                values: new object[,]
                {
                    { 1, 1, 2, "All", 1, "Id_Of_It_Dept", "1" },
                    { 2, 4, 3, "All", 1, null, "2" },
                    { 3, 2, 2, "All", 1, "ID_Of_North_Region", "3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "AccessAttributeTables",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "DataPermissions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "DataPermissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "DataPermissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "AccessAttributeTables",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "AccessAttributeTables",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "AccessAttributeTables",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "AccessAttributes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "DataAuth",
                table: "AccessAttributes",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
