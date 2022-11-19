using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DataLayer.Migrations
{
    public partial class samplejwt_03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Uid",
                table: "Tbl_Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uid",
                table: "Tbl_Users");
        }
    }
}
