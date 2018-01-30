using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace tabletop.Migrations
{
    public partial class AddBearerToChannelUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NameUrlSafe",
                table: "ChannelUser",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 80,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bearer",
                table: "ChannelUser",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bearer",
                table: "ChannelUser");

            migrationBuilder.AlterColumn<string>(
                name: "NameUrlSafe",
                table: "ChannelUser",
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 80);
        }
    }
}
