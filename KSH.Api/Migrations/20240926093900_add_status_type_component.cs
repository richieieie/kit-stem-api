using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSH.Api.Migrations
{
    /// <inheritdoc />
    public partial class add_status_type_component : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "ComponentsType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Component",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ComponentsType");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Component");
        }
    }
}
