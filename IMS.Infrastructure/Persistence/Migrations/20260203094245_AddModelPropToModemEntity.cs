using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddModelPropToModemEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Modems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Model",
                table: "Modems");
        }
    }
}
