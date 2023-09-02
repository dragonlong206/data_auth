using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAuth.Sample.WebApi.Migrations.DataAuthDb
{
    /// <inheritdoc />
    public partial class AddFunctionCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FunctionCode",
                schema: "DataAuth",
                table: "DataPermissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "All");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FunctionCode",
                schema: "DataAuth",
                table: "DataPermissions");
        }
    }
}
