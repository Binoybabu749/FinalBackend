using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_food_delivery_system.Migrations
{
    /// <inheritdoc />
    public partial class dbcontext_Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Payments_OrderID",
                table: "Deliveries");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryID",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DeliveryID",
                table: "Payments",
                column: "DeliveryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Deliveries_DeliveryID",
                table: "Payments",
                column: "DeliveryID",
                principalTable: "Deliveries",
                principalColumn: "DeliveryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Deliveries_DeliveryID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_DeliveryID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeliveryID",
                table: "Payments");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Payments_OrderID",
                table: "Deliveries",
                column: "OrderID",
                principalTable: "Payments",
                principalColumn: "PaymentID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
