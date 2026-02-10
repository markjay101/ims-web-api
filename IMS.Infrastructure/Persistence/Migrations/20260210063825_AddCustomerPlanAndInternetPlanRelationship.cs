using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerPlanAndInternetPlanRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CustomerPlans_InternetPlanId",
                table: "CustomerPlans",
                column: "InternetPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPlans_InternetPlans_InternetPlanId",
                table: "CustomerPlans",
                column: "InternetPlanId",
                principalTable: "InternetPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPlans_InternetPlans_InternetPlanId",
                table: "CustomerPlans");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPlans_InternetPlanId",
                table: "CustomerPlans");
        }
    }
}
