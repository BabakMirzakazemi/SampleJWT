using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DataLayer.Migrations
{
    public partial class samplejwt_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Tbl_Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Tbl_Users");
        }
    }
}
