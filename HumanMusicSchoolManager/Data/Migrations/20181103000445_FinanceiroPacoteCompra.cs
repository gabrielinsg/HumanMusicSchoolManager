using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Data.Migrations
{
    public partial class FinanceiroPacoteCompra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Desconto",
                table: "PacoteCompras",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "PacoteCompraId",
                table: "Financeiros",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Financeiros_PacoteCompraId",
                table: "Financeiros",
                column: "PacoteCompraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Financeiros_PacoteCompras_PacoteCompraId",
                table: "Financeiros",
                column: "PacoteCompraId",
                principalTable: "PacoteCompras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financeiros_PacoteCompras_PacoteCompraId",
                table: "Financeiros");

            migrationBuilder.DropIndex(
                name: "IX_Financeiros_PacoteCompraId",
                table: "Financeiros");

            migrationBuilder.DropColumn(
                name: "PacoteCompraId",
                table: "Financeiros");

            migrationBuilder.AlterColumn<decimal>(
                name: "Desconto",
                table: "PacoteCompras",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
