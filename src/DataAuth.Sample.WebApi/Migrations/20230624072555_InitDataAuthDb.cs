using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAuth.Sample.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitDataAuthDb : Migration
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
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    HierarchyLevel = table.Column<int>(type: "int", nullable: false),
                    LocalPermissionTableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalPermissionIdColumn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalPermissionLookupColumn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessAttributeTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessAttributeTables_AccessAttributes_AccessAttributeId",
                        column: x => x.AccessAttributeId,
                        principalTable: "AccessAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.ForeignKey(
                        name: "FK_DataPermissions_AccessAttributeTables_AccessAttributeTableId",
                        column: x => x.AccessAttributeTableId,
                        principalTable: "AccessAttributeTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessAttributes_Code",
                table: "AccessAttributes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessAttributeTables_AccessAttributeId",
                table: "AccessAttributeTables",
                column: "AccessAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataPermissions_AccessAttributeTableId",
                table: "DataPermissions",
                column: "AccessAttributeTableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataPermissions");

            migrationBuilder.DropTable(
                name: "AccessAttributeTables");

            migrationBuilder.DropTable(
                name: "AccessAttributes");
        }
    }
}
