using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kit_stem_api.Migrations
{
    /// <inheritdoc />
    public partial class Modified_Tables_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "KitsCategory",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "KitImage",
                newName: "Image");

            migrationBuilder.RenameTable(
                name: "ComponentsType",
                newName: "Type");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Package");

            migrationBuilder.RenameTable(
                name: "Type",
                newName: "ComponentsType");

            migrationBuilder.RenameTable(
                name: "Image",
                newName: "KitImage");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "KitsCategory");

            migrationBuilder.RenameIndex(
                name: "IX_Image_KitId",
                table: "KitImage",
                newName: "IX_KitImage_KitId");
        }
    }
}
