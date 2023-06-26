using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAuth.Sample.WebApi.Migrations.DataAuthDb
{
    /// <inheritdoc />
    public partial class InitDataAuthDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataAuth_AccessAttributes",
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
                    table.PrimaryKey("PK_DataAuth_AccessAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataAuth_AccessAttributeTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessAttributeId = table.Column<int>(type: "int", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdColumn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameColumn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentColumn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSelfReference = table.Column<bool>(type: "bit", nullable: false),
                    HierarchyLevel = table.Column<int>(type: "int", nullable: true),
                    LocalPermissionTableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalPermissionIdColumn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalPermissionLookupColumn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataAuth_AccessAttributeTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataAuth_AccessAttributeTables_DataAuth_AccessAttributes_AccessAttributeId",
                        column: x => x.AccessAttributeId,
                        principalTable: "DataAuth_AccessAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataAuth_DataPermissions",
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
                    table.PrimaryKey("PK_DataAuth_DataPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataAuth_DataPermissions_DataAuth_AccessAttributeTables_AccessAttributeTableId",
                        column: x => x.AccessAttributeTableId,
                        principalTable: "DataAuth_AccessAttributeTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataAuth_AccessAttributes_Code",
                table: "DataAuth_AccessAttributes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataAuth_AccessAttributeTables_AccessAttributeId",
                table: "DataAuth_AccessAttributeTables",
                column: "AccessAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataAuth_AccessAttributeTables_Alias",
                table: "DataAuth_AccessAttributeTables",
                column: "Alias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataAuth_DataPermissions_AccessAttributeTableId",
                table: "DataAuth_DataPermissions",
                column: "AccessAttributeTableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataAuth_DataPermissions");

            migrationBuilder.DropTable(
                name: "DataAuth_AccessAttributeTables");

            migrationBuilder.DropTable(
                name: "DataAuth_AccessAttributes");
        }
    }
}
