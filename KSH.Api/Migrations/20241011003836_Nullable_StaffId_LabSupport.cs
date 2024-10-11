using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KST.Api.Migrations
{
    /// <inheritdoc />
    public partial class Nullable_StaffId_LabSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabSupport_AspNetUsers_StaffId",
                table: "LabSupport");

            migrationBuilder.AlterColumn<string>(
                name: "StaffId",
                table: "LabSupport",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_LabSupport_AspNetUsers_StaffId",
                table: "LabSupport",
                column: "StaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabSupport_AspNetUsers_StaffId",
                table: "LabSupport");

            migrationBuilder.AlterColumn<string>(
                name: "StaffId",
                table: "LabSupport",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LabSupport_AspNetUsers_StaffId",
                table: "LabSupport",
                column: "StaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
