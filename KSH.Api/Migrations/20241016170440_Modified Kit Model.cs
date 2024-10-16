using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSH.Api.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedKitModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MaxPackagePrice",
                table: "Kit",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MinPackagePrice",
                table: "Kit",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPackagePrice",
                table: "Kit");

            migrationBuilder.DropColumn(
                name: "MinPackagePrice",
                table: "Kit");
        }
    }
}
