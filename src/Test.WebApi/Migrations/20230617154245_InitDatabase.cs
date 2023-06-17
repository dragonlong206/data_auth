using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessAttributeTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessAttributeId = table.Column<int>(type: "int", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdColumn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameColumn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentColumn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HiearchyLevel = table.Column<int>(type: "int", nullable: false),
                    LocalPermissionTableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalPermissionIdColumn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalPermissionLookupColumn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessAttributeTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrantType = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccessAttributeTableId = table.Column<int>(type: "int", nullable: false),
                    AccessLevel = table.Column<int>(type: "int", nullable: false),
                    GrantedDataValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPermissions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessAttributes");

            migrationBuilder.DropTable(
                name: "AccessAttributeTables");

            migrationBuilder.DropTable(
                name: "DataPermissions");
        }
    }
}
