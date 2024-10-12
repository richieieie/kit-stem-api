using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSH.Api.Migrations
{
    /// <inheritdoc />
    public partial class Update_Constraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Kit__CategoryId__6E01572D",
                table: "Kit");

            migrationBuilder.DropForeignKey(
                name: "FK__Payment__MethodI__0F624AF8",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK__UserOrders__Shipp__17F790F9",
                table: "UserOrders");

            migrationBuilder.DropTable(
                name: "ShippingStatus");

            migrationBuilder.DropIndex(
                name: "IX_UserOrders_ShippingStatusId",
                table: "UserOrders");

            migrationBuilder.DropColumn(
                name: "IsKitDelivered",
                table: "UserOrders");

            migrationBuilder.DropColumn(
                name: "ShippingStatusId",
                table: "UserOrders");

            migrationBuilder.DropColumn(
                name: "IsComponentStored",
                table: "KitComponent");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Kit");

            migrationBuilder.DropColumn(
                name: "QuantityInStore",
                table: "Component");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "UserOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingStatus",
                table: "UserOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "MethodId",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "Package",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lab",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "KitsCategory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Kit",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Kit",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Kit",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Kit",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Brief",
                table: "Kit",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Package_LevelId",
                table: "Package",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK__Kit__CategoryId__6E01572D",
                table: "Kit",
                column: "CategoryId",
                principalTable: "KitsCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Package_Level_LevelId",
                table: "Package",
                column: "LevelId",
                principalTable: "Level",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Payment__MethodI__0F624AF8",
                table: "Payment",
                column: "MethodId",
                principalTable: "Method",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Kit__CategoryId__6E01572D",
                table: "Kit");

            migrationBuilder.DropForeignKey(
                name: "FK_Package_Level_LevelId",
                table: "Package");

            migrationBuilder.DropForeignKey(
                name: "FK__Payment__MethodI__0F624AF8",
                table: "Payment");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_Package_LevelId",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "ShippingStatus",
                table: "UserOrders");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "Package");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "UserOrders",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsKitDelivered",
                table: "UserOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ShippingStatusId",
                table: "UserOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MethodId",
                table: "Payment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lab",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "KitsCategory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsComponentStored",
                table: "KitComponent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Kit",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Kit",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Kit",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Kit",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Brief",
                table: "Kit",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Kit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityInStore",
                table: "Component",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ShippingStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shipping__3214EC0740CF1637", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserOrders_ShippingStatusId",
                table: "UserOrders",
                column: "ShippingStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK__Kit__CategoryId__6E01572D",
                table: "Kit",
                column: "CategoryId",
                principalTable: "KitsCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Payment__MethodI__0F624AF8",
                table: "Payment",
                column: "MethodId",
                principalTable: "Method",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__UserOrders__Shipp__17F790F9",
                table: "UserOrders",
                column: "ShippingStatusId",
                principalTable: "ShippingStatus",
                principalColumn: "Id");
        }
    }
}
