using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KST.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabSupporters_AspNetUsers_StaffId",
                table: "LabSupporters");

            migrationBuilder.DropForeignKey(
                name: "FK_LabSupporters_LabSupport_LabSupportId",
                table: "LabSupporters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabSupporters",
                table: "LabSupporters");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "RefreshToken");

            migrationBuilder.RenameTable(
                name: "LabSupporters",
                newName: "LabSupporter");

            migrationBuilder.RenameTable(
                name: "KitImages",
                newName: "KitImage");

            migrationBuilder.RenameIndex(
                name: "IX_LabSupporters_StaffId",
                table: "LabSupporter",
                newName: "IX_LabSupporter_StaffId");

            migrationBuilder.RenameIndex(
                name: "IX_LabSupporters_LabSupportId",
                table: "LabSupporter",
                newName: "IX_LabSupporter_LabSupportId");

            migrationBuilder.RenameIndex(
                name: "IX_KitImages_KitId",
                table: "KitImage",
                newName: "IX_KitImage_KitId");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabSupporter",
                table: "LabSupporter",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabSupporter_AspNetUsers_StaffId",
                table: "LabSupporter",
                column: "StaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabSupporter_LabSupport_LabSupportId",
                table: "LabSupporter",
                column: "LabSupportId",
                principalTable: "LabSupport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabSupporter_AspNetUsers_StaffId",
                table: "LabSupporter");

            migrationBuilder.DropForeignKey(
                name: "FK_LabSupporter_LabSupport_LabSupportId",
                table: "LabSupporter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabSupporter",
                table: "LabSupporter");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "RefreshToken",
                newName: "RefreshTokens");

            migrationBuilder.RenameTable(
                name: "LabSupporter",
                newName: "LabSupporters");

            migrationBuilder.RenameTable(
                name: "KitImage",
                newName: "KitImages");

            migrationBuilder.RenameIndex(
                name: "IX_LabSupporter_StaffId",
                table: "LabSupporters",
                newName: "IX_LabSupporters_StaffId");

            migrationBuilder.RenameIndex(
                name: "IX_LabSupporter_LabSupportId",
                table: "LabSupporters",
                newName: "IX_LabSupporters_LabSupportId");

            migrationBuilder.RenameIndex(
                name: "IX_KitImage_KitId",
                table: "KitImages",
                newName: "IX_KitImages_KitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabSupporters",
                table: "LabSupporters",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabSupporters_AspNetUsers_StaffId",
                table: "LabSupporters",
                column: "StaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabSupporters_LabSupport_LabSupportId",
                table: "LabSupporters",
                column: "LabSupportId",
                principalTable: "LabSupport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
