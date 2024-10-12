using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSH.Api.Migrations
{
    /// <inheritdoc />
    public partial class Add_Cart_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabSupport_Package_PackageId",
                table: "LabSupport");

            migrationBuilder.DropForeignKey(
                name: "FK__LabSuppor__LabId__22751F6C",
                table: "LabSupport");

            migrationBuilder.DropForeignKey(
                name: "FK__LabSuppor__Order__236943A5",
                table: "LabSupport");

            migrationBuilder.DropTable(
                name: "LabSupporter");

            migrationBuilder.DropIndex(
                name: "IX_PackageOrder_PackageId",
                table: "PackageOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK__LabSuppor__01846D66652C0B0E",
                table: "LabSupport");

            migrationBuilder.DropIndex(
                name: "IX_LabSupport_LabId_PackageId_OrderId",
                table: "LabSupport");

            migrationBuilder.DropIndex(
                name: "IX_LabSupport_PackageId",
                table: "LabSupport");

            migrationBuilder.DropIndex(
                name: "IX_UserOrders_PaymentId",
                table: "UserOrders");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "LabSupport");

            migrationBuilder.RenameTable(
                name: "UserOrders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "KitsCategory",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "KitImage",
                newName: "Image");

            migrationBuilder.RenameTable(
                name: "ComponentsType",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "RemainSupportTimes",
                table: "LabSupport",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "LabSupport",
                newName: "OrderSupportId");

            migrationBuilder.RenameColumn(
                name: "LabId",
                table: "LabSupport",
                newName: "LabSupportId");

            migrationBuilder.RenameIndex(
                name: "IX_LabSupport_OrderId",
                table: "LabSupport",
                newName: "IX_LabSupport_OrderSupportId");

            migrationBuilder.RenameIndex(
                name: "IX_UserOrders_UserId",
                table: "Order",
                newName: "IX_Order_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_KitImage_KitId",
                table: "Image",
                newName: "IX_Image_KitId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Package",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "LabSupport",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "FeedBack",
                table: "LabSupport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "LabSupport",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StaffId",
                table: "LabSupport",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackageOrder",
                table: "PackageOrder",
                columns: new[] { "PackageId", "OrderId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabSupport",
                table: "LabSupport",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    PackageQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => new { x.UserId, x.PackageId });
                    table.ForeignKey(
                        name: "FK__Cart__PackageId__POSI3213AAL",
                        column: x => x.PackageId,
                        principalTable: "Package",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Cart__UserId__AB35320CD",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderSupport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LabId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    RemainSupportTimes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LabSuppor__01846D66652C0B0E", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderSupport_Package_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Package",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__LabSuppor__LabId__22751F6C",
                        column: x => x.LabId,
                        principalTable: "Lab",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__LabSuppor__Order__236943A5",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabSupport_StaffId",
                table: "LabSupport",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentId",
                table: "Order",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cart_PackageId",
                table: "Cart",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSupport_LabId_PackageId_OrderId",
                table: "OrderSupport",
                columns: new[] { "LabId", "PackageId", "OrderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderSupport_OrderId",
                table: "OrderSupport",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSupport_PackageId",
                table: "OrderSupport",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabSupport_AspNetUsers_StaffId",
                table: "LabSupport",
                column: "StaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabSupport_OrderSupport_OrderSupportId",
                table: "LabSupport",
                column: "OrderSupportId",
                principalTable: "OrderSupport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabSupport_AspNetUsers_StaffId",
                table: "LabSupport");

            migrationBuilder.DropForeignKey(
                name: "FK_LabSupport_OrderSupport_OrderSupportId",
                table: "LabSupport");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "OrderSupport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PackageOrder",
                table: "PackageOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabSupport",
                table: "LabSupport");

            migrationBuilder.DropIndex(
                name: "IX_LabSupport_StaffId",
                table: "LabSupport");

            migrationBuilder.DropIndex(
                name: "IX_Order_PaymentId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "LabSupport");

            migrationBuilder.DropColumn(
                name: "FeedBack",
                table: "LabSupport");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "LabSupport");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "LabSupport");

            migrationBuilder.RenameTable(
                name: "Type",
                newName: "ComponentsType");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "UserOrders");

            migrationBuilder.RenameTable(
                name: "Image",
                newName: "KitImage");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "KitsCategory");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "LabSupport",
                newName: "RemainSupportTimes");

            migrationBuilder.RenameColumn(
                name: "OrderSupportId",
                table: "LabSupport",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "LabSupportId",
                table: "LabSupport",
                newName: "LabId");

            migrationBuilder.RenameIndex(
                name: "IX_LabSupport_OrderSupportId",
                table: "LabSupport",
                newName: "IX_LabSupport_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_UserId",
                table: "UserOrders",
                newName: "IX_UserOrders_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Image_KitId",
                table: "KitImage",
                newName: "IX_KitImage_KitId");

            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "LabSupport",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK__LabSuppor__01846D66652C0B0E",
                table: "LabSupport",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "LabSupporter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LabSupportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FeedBack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabSupporter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabSupporter_AspNetUsers_StaffId",
                        column: x => x.StaffId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LabSupporter_LabSupport_LabSupportId",
                        column: x => x.LabSupportId,
                        principalTable: "LabSupport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageOrder_PackageId",
                table: "PackageOrder",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_LabSupport_LabId_PackageId_OrderId",
                table: "LabSupport",
                columns: new[] { "LabId", "PackageId", "OrderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabSupport_PackageId",
                table: "LabSupport",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrders_PaymentId",
                table: "UserOrders",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_LabSupporter_LabSupportId",
                table: "LabSupporter",
                column: "LabSupportId");

            migrationBuilder.CreateIndex(
                name: "IX_LabSupporter_StaffId",
                table: "LabSupporter",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabSupport_Package_PackageId",
                table: "LabSupport",
                column: "PackageId",
                principalTable: "Package",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__LabSuppor__LabId__22751F6C",
                table: "LabSupport",
                column: "LabId",
                principalTable: "Lab",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__LabSuppor__Order__236943A5",
                table: "LabSupport",
                column: "OrderId",
                principalTable: "UserOrders",
                principalColumn: "Id");
        }
    }
}
