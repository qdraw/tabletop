using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tabletop.Migrations
{
    public partial class addActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelActivity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChannelUserId = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Success = table.Column<bool>(nullable: false),
                    TimeSpan = table.Column<TimeSpan>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelActivity_ChannelUser_ChannelUserId",
                        column: x => x.ChannelUserId,
                        principalTable: "ChannelUser",
                        principalColumn: "NameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelActivity_ChannelUserId",
                table: "ChannelActivity",
                column: "ChannelUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelActivity");
        }
    }
}
