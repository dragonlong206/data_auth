using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAuth.Sample.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedingSampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Code", "Name", "ParentDepartmentId" },
                values: new object[] { 1, "KDO", "Phòng Kinh doanh", null });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "Miền Bắc", "MB" },
                    { 2, "Miền Trung", "MT" },
                    { 3, "Miền Nam", "MN" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Code", "Name", "ParentDepartmentId" },
                values: new object[] { 2, "KDO_CHA", "Bộ phận Cửa hàng", 1 });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "Id", "Code", "Name", "RegionId" },
                values: new object[,]
                {
                    { 1, "HCM", "Hồ Chí Minh", 3 },
                    { 2, "TNI", "Tây Ninh", 3 },
                    { 3, "BDU", "Bình Dương", 3 },
                    { 4, "DNI", "Đồng Nai", 3 }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Code", "Name", "ParentDepartmentId" },
                values: new object[,]
                {
                    { 3, "KDO_NVU", "Bộ phận Nghiệp vụ", 2 },
                    { 6, "KDO_CHA_NVB", "Nhân viên bán hàng", 2 }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "Code", "Name", "ProvinceId" },
                values: new object[,]
                {
                    { 1, "HCM_001", "Cửa hàng Trần Hưng Đạo", 1 },
                    { 2, "HCM_002", "Cửa hàng Nguyễn Văn Cừ", 1 },
                    { 3, "HCM_003", "Cửa hàng Nguyễn Oanh", 1 },
                    { 4, "HCM_004", "Cửa hàng Phan Đăng Lưu", 1 },
                    { 5, "HCM_005", "Cửa hàng Võ Văn Việt", 1 },
                    { 6, "DNI_001", "Cửa hàng Biên Hòa 1", 4 },
                    { 7, "BDU_001", "Cửa hàng Dĩ An", 3 }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Code", "Name", "ParentDepartmentId" },
                values: new object[,]
                {
                    { 4, "KDO_NVU_VTU", "Bộ phận Vật tư", 3 },
                    { 5, "KDO_NVU_CTU", "Bộ phận Chứng từ hóa đơn", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
