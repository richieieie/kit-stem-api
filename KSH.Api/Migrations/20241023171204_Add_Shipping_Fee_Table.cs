using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KSH.Api.Migrations
{
    /// <inheritdoc />
    public partial class Add_Shipping_Fee_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShippingFeeId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ShippingFee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromDistance = table.Column<int>(type: "int", nullable: false),
                    ToDistance = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingFee", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ShippingFee",
                columns: new[] { "Id", "FromDistance", "Price", "ToDistance" },
                values: new object[,]
                {
                    { 1, 0, 20000L, 5 },
                    { 2, 6, 30000L, 10 },
                    { 3, 11, 40000L, 20 },
                    { 4, 21, 60000L, 30 },
                    { 5, 31, 80000L, 50 },
                    { 6, 51, 120000L, 100 },
                    { 7, 101, 180000L, 2147483647 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShippingFeeId",
                table: "Order",
                column: "ShippingFeeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_ShippingFee_ShippingFeeId",
                table: "Order",
                column: "ShippingFeeId",
                principalTable: "ShippingFee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_ShippingFee_ShippingFeeId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "ShippingFee");

            migrationBuilder.DropIndex(
                name: "IX_Order_ShippingFeeId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShippingFeeId",
                table: "Order");
        }
    }
}
