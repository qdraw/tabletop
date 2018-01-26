using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace tabletop.Migrations
{
    public partial class newRelationalDatabaseLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelEvent_ChannelUser_NameId",
                table: "ChannelEvent");

            migrationBuilder.RenameColumn(
                name: "NameId",
                table: "ChannelEvent",
                newName: "ChannelUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelEvent_NameId",
                table: "ChannelEvent",
                newName: "IX_ChannelEvent_ChannelUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelEvent_ChannelUser_ChannelUserId",
                table: "ChannelEvent",
                column: "ChannelUserId",
                principalTable: "ChannelUser",
                principalColumn: "NameId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelEvent_ChannelUser_ChannelUserId",
                table: "ChannelEvent");

            migrationBuilder.RenameColumn(
                name: "ChannelUserId",
                table: "ChannelEvent",
                newName: "NameId");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelEvent_ChannelUserId",
                table: "ChannelEvent",
                newName: "IX_ChannelEvent_NameId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelEvent_ChannelUser_NameId",
                table: "ChannelEvent",
                column: "NameId",
                principalTable: "ChannelUser",
                principalColumn: "NameId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
