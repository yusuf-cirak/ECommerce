using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Persistance.Migrations
{
    public partial class basket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Baskets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "TotalPrice",
                table: "Baskets",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
