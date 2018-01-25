using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace tabletop.Migrations
{
    public partial class AddFirstChannelUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Channel(IsVisible,Name,NameId) VALUES('true','tafelvoetbal','0000-0000-0000-0000-0000' )");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Channel WHERE Name IN ('tafelvoetbal')");
        }
    }
}
