using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marboket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveForPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Prices",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Prices");
        }
    }
}
