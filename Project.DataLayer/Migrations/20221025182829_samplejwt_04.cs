using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DataLayer.Migrations
{
    public partial class samplejwt_04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tbl_Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tbl_Users");
        }
    }
}
