using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_food_delivery_system.Migrations
{
    /// <inheritdoc />
    public partial class lastmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderMenuItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OrderMenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
