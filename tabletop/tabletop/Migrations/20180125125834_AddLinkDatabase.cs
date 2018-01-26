using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace tabletop.Migrations
{
    public partial class AddLinkDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UpdateStatus");

            migrationBuilder.AlterColumn<string>(
                name: "NameId",
                table: "Channel",
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdateStatusId",
                table: "Channel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channel_UpdateStatusId",
                table: "Channel",
                column: "UpdateStatusId",
                unique: true,
                filter: "[UpdateStatusId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_UpdateStatus_UpdateStatusId",
                table: "Channel",
                column: "UpdateStatusId",
                principalTable: "UpdateStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_UpdateStatus_UpdateStatusId",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_UpdateStatusId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "UpdateStatusId",
                table: "Channel");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UpdateStatus",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "NameId",
                table: "Channel",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 80,
                oldNullable: true);
        }
    }
}
