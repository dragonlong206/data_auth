using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAuth.Sample.WebApi.Migrations.DataAuthDb
{
    /// <inheritdoc />
    public partial class ChangeRoleIdInUserRoleToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataAuth_UserRoles_DataAuth_Roles_RoleId1",
                table: "DataAuth_UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_DataAuth_UserRoles_RoleId1",
                table: "DataAuth_UserRoles");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                table: "DataAuth_UserRoles");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "DataAuth_UserRoles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_DataAuth_UserRoles_RoleId",
                table: "DataAuth_UserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataAuth_UserRoles_DataAuth_Roles_RoleId",
                table: "DataAuth_UserRoles",
                column: "RoleId",
                principalTable: "DataAuth_Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataAuth_UserRoles_DataAuth_Roles_RoleId",
                table: "DataAuth_UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_DataAuth_UserRoles_RoleId",
                table: "DataAuth_UserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "DataAuth_UserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RoleId1",
                table: "DataAuth_UserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataAuth_UserRoles_RoleId1",
                table: "DataAuth_UserRoles",
                column: "RoleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DataAuth_UserRoles_DataAuth_Roles_RoleId1",
                table: "DataAuth_UserRoles",
                column: "RoleId1",
                principalTable: "DataAuth_Roles",
                principalColumn: "Id");
        }
    }
}
