using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAuth.Sample.WebApi.Migrations.DataAuthDb
{
    /// <inheritdoc />
    public partial class AddUserRoleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataAuth_Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataAuth_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataAuth_UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataAuth_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataAuth_UserRoles_DataAuth_Roles_RoleId1",
                        column: x => x.RoleId1,
                        principalTable: "DataAuth_Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataAuth_Roles_Code",
                table: "DataAuth_Roles",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataAuth_UserRoles_RoleId1",
                table: "DataAuth_UserRoles",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_DataAuth_UserRoles_UserId_RoleId",
                table: "DataAuth_UserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataAuth_UserRoles");

            migrationBuilder.DropTable(
                name: "DataAuth_Roles");
        }
    }
}
