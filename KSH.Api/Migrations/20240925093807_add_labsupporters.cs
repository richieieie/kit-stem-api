using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KST.Api.Migrations
{
    /// <inheritdoc />
    public partial class add_labsupporters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ__Level__A93C97B9E3009846",
                table: "Level");

            migrationBuilder.DropPrimaryKey(
                name: "PK__LabSuppo__01846D66652C0B0E",
                table: "LabSupport");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Level");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "LabSupport",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "LabSupport",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "KitsCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK__LabSuppor__01846D66652C0B0E",
                table: "LabSupport",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "LabSupporters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LabSupportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    FeedBack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabSupporters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabSupporters_AspNetUsers_StaffId",
                        column: x => x.StaffId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LabSupporters_LabSupport_LabSupportId",
                        column: x => x.LabSupportId,
                        principalTable: "LabSupport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_LabSupporters_LabSupportId",
                table: "LabSupporters",
                column: "LabSupportId");

            migrationBuilder.CreateIndex(
                name: "IX_LabSupporters_StaffId",
                table: "LabSupporters",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabSupport_Package_PackageId",
                table: "LabSupport",
                column: "PackageId",
                principalTable: "Package",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabSupport_Package_PackageId",
                table: "LabSupport");

            migrationBuilder.DropTable(
                name: "LabSupporters");

            migrationBuilder.DropPrimaryKey(
                name: "PK__LabSuppor__01846D66652C0B0E",
                table: "LabSupport");

            migrationBuilder.DropIndex(
                name: "IX_LabSupport_LabId_PackageId_OrderId",
                table: "LabSupport");

            migrationBuilder.DropIndex(
                name: "IX_LabSupport_PackageId",
                table: "LabSupport");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LabSupport");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "LabSupport");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "KitsCategory");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Level",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK__LabSuppo__01846D66652C0B0E",
                table: "LabSupport",
                columns: new[] { "LabId", "OrderId" });

            migrationBuilder.CreateIndex(
                name: "UQ__Level__A93C97B9E3009846",
                table: "Level",
                column: "NormalizedName",
                unique: true);
        }
    }
}
