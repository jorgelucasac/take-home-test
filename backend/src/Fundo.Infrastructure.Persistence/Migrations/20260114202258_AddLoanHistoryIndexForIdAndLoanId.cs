using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fundo.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLoanHistoryIndexForIdAndLoanId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LoanHistories_LoanId",
                table: "LoanHistories");

            migrationBuilder.CreateIndex(
                name: "IX_LoanHistories_LoanId_Id",
                table: "LoanHistories",
                columns: new[] { "LoanId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LoanHistories_LoanId_Id",
                table: "LoanHistories");

            migrationBuilder.CreateIndex(
                name: "IX_LoanHistories_LoanId",
                table: "LoanHistories",
                column: "LoanId");
        }
    }
}
