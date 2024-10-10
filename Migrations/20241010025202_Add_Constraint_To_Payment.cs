using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kit_stem_api.Migrations
{
    /// <inheritdoc />
    public partial class Add_Constraint_To_Payment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__UserOrders__Payme__160F4887",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_PaymentId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Order");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Payment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Payment_OrderId",
                table: "Payment",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Order_OrderId",
                table: "Payment",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Order_OrderId",
                table: "Payment");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Payment_OrderId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Payment");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentId",
                table: "Order",
                column: "PaymentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__UserOrders__Payme__160F4887",
                table: "Order",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id");
        }
    }
}
