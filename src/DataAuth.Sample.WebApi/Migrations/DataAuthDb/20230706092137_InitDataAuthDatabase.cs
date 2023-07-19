using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAuth.Sample.WebApi.Migrations.DataAuthDb
{
    /// <inheritdoc />
    public partial class InitDataAuthDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "DataAuth");

            migrationBuilder.CreateTable(
                name: "AccessAttributes",
                schema: "DataAuth",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessAttributes", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "DataAuth",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                        Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "AccessAttributeTables",
                schema: "DataAuth",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        AccessAttributeId = table.Column<int>(type: "int", nullable: false),
                        TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Alias = table.Column<string>(type: "nvarchar(450)", nullable: false),
                        IdColumn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        NameColumn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        ParentColumn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        IsSelfReference = table.Column<bool>(type: "bit", nullable: false),
                        HierarchyLevel = table.Column<int>(type: "int", nullable: true),
                        IsLeafLevel = table.Column<bool>(type: "bit", nullable: false),
                        LocalPermissionTableName = table.Column<string>(
                            type: "nvarchar(max)",
                            nullable: true
                        ),
                        LocalPermissionIdColumn = table.Column<string>(
                            type: "nvarchar(max)",
                            nullable: true
                        ),
                        LocalPermissionLookupColumn = table.Column<string>(
                            type: "nvarchar(max)",
                            nullable: true
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessAttributeTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessAttributeTables_AccessAttributes_AccessAttributeId",
                        column: x => x.AccessAttributeId,
                        principalSchema: "DataAuth",
                        principalTable: "AccessAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "DataAuth",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                        RoleId = table.Column<int>(type: "int", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "DataAuth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "DataPermissions",
                schema: "DataAuth",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        GrantType = table.Column<int>(type: "int", nullable: false),
                        SubjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        AccessAttributeTableId = table.Column<int>(type: "int", nullable: false),
                        AccessLevel = table.Column<int>(type: "int", nullable: false),
                        GrantedDataValue = table.Column<string>(
                            type: "nvarchar(max)",
                            nullable: true
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataPermissions_AccessAttributeTables_AccessAttributeTableId",
                        column: x => x.AccessAttributeTableId,
                        principalSchema: "DataAuth",
                        principalTable: "AccessAttributeTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_AccessAttributes_Code",
                schema: "DataAuth",
                table: "AccessAttributes",
                column: "Code",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_AccessAttributeTables_AccessAttributeId",
                schema: "DataAuth",
                table: "AccessAttributeTables",
                column: "AccessAttributeId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_AccessAttributeTables_Alias",
                schema: "DataAuth",
                table: "AccessAttributeTables",
                column: "Alias",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_DataPermissions_AccessAttributeTableId",
                schema: "DataAuth",
                table: "DataPermissions",
                column: "AccessAttributeTableId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Code",
                schema: "DataAuth",
                table: "Roles",
                column: "Code",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "DataAuth",
                table: "UserRoles",
                column: "RoleId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_RoleId",
                schema: "DataAuth",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "DataPermissions", schema: "DataAuth");

            migrationBuilder.DropTable(name: "UserRoles", schema: "DataAuth");

            migrationBuilder.DropTable(name: "AccessAttributeTables", schema: "DataAuth");

            migrationBuilder.DropTable(name: "Roles", schema: "DataAuth");

            migrationBuilder.DropTable(name: "AccessAttributes", schema: "DataAuth");
        }
    }
}
