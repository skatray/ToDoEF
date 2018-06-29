using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoEF.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TablegroupSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablegroupSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TablelistSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TaskName = table.Column<string>(nullable: true),
                    DateStart = table.Column<DateTime>(nullable: true),
                    DateEnd = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablelistSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TablelistTablegroup",
                columns: table => new
                {
                    TablelistId = table.Column<int>(nullable: false),
                    TablegroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablelistTablegroup", x => new { x.TablelistId, x.TablegroupId });
                    table.ForeignKey(
                        name: "FK_TablelistTablegroup_TablelistSet_TablegroupId",
                        column: x => x.TablegroupId,
                        principalTable: "TablelistSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TablelistTablegroup_TablegroupSet_TablelistId",
                        column: x => x.TablelistId,
                        principalTable: "TablegroupSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TablelistTablegroup_TablegroupId",
                table: "TablelistTablegroup",
                column: "TablegroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TablelistTablegroup");

            migrationBuilder.DropTable(
                name: "TablelistSet");

            migrationBuilder.DropTable(
                name: "TablegroupSet");
        }
    }
}
