using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kit_stem_api.Migrations
{
    /// <inheritdoc />
    public partial class Add_OrderId_To_Payment : Migration
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

            migrationBuilder.AddColumn<Guid>(
                name: "UserOrdersId",
                table: "Payment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserOrdersId",
                table: "Payment",
                column: "UserOrdersId");

            migrationBuilder.AddForeignKey(
                name: "FK__Payment__UserOr__123451AF",
                table: "Payment",
                column: "UserOrdersId",
                principalTable: "Order",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Payment__UserOr__123451AF",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_UserOrdersId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "UserOrdersId",
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
