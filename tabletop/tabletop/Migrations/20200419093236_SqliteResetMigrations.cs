using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tabletop.Migrations
{
    public partial class SqliteResetMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelUser",
                columns: table => new
                {
                    NameId = table.Column<string>(maxLength: 80, nullable: false),
                    Name = table.Column<string>(maxLength: 80, nullable: false),
                    NameUrlSafe = table.Column<string>(maxLength: 80, nullable: false),
                    IsVisible = table.Column<bool>(nullable: false),
                    IsAccessible = table.Column<bool>(nullable: false),
                    Bearer = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelUser", x => x.NameId);
                });

            migrationBuilder.CreateTable(
                name: "ChannelEvent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<int>(nullable: false),
                    ChannelUserId = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Weight = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelEvent_ChannelUser_ChannelUserId",
                        column: x => x.ChannelUserId,
                        principalTable: "ChannelUser",
                        principalColumn: "NameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelEvent_ChannelUserId",
                table: "ChannelEvent",
                column: "ChannelUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelEvent");

            migrationBuilder.DropTable(
                name: "ChannelUser");
        }
    }
}
