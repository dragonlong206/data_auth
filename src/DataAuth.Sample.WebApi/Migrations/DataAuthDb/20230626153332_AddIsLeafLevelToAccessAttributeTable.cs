using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAuth.Sample.WebApi.Migrations.DataAuthDb
{
    /// <inheritdoc />
    public partial class AddIsLeafLevelToAccessAttributeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLeafLevel",
                table: "DataAuth_AccessAttributeTables",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLeafLevel",
                table: "DataAuth_AccessAttributeTables");
        }
    }
}
