using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace tabletop.Migrations
{
    public partial class AddLinkDatabase4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Channel");

            migrationBuilder.DropTable(
                name: "UpdateStatus");

            migrationBuilder.CreateTable(
                name: "ChannelUser",
                columns: table => new
                {
                    NameId = table.Column<string>(maxLength: 80, nullable: false),
                    IsVisible = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 80, nullable: false)
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    NameId = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelEvent_ChannelUser_NameId",
                        column: x => x.NameId,
                        principalTable: "ChannelUser",
                        principalColumn: "NameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelEvent_NameId",
                table: "ChannelEvent",
                column: "NameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelEvent");

            migrationBuilder.DropTable(
                name: "ChannelUser");

            migrationBuilder.CreateTable(
                name: "UpdateStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Channel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsVisible = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 80, nullable: false),
                    NameId = table.Column<string>(maxLength: 80, nullable: true),
                    UpdateStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Channel_UpdateStatus_UpdateStatusId",
                        column: x => x.UpdateStatusId,
                        principalTable: "UpdateStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Channel_UpdateStatusId",
                table: "Channel",
                column: "UpdateStatusId",
                unique: true,
                filter: "[UpdateStatusId] IS NOT NULL");
        }
    }
}
