using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kit_stem_api.Migrations
{
    /// <inheritdoc />
    public partial class change_component_constraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Component_Name",
                table: "Component",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Component_Name",
                table: "Component");
        }
    }
}
