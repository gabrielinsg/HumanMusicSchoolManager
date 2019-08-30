using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class CaixaCaixaAnterior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorAberto",
                table: "Caixas");

            migrationBuilder.AddColumn<int>(
                name: "CaixaAnteriorId",
                table: "Caixas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Caixas_CaixaAnteriorId",
                table: "Caixas",
                column: "CaixaAnteriorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Caixas_Caixas_CaixaAnteriorId",
                table: "Caixas",
                column: "CaixaAnteriorId",
                principalTable: "Caixas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Caixas_Caixas_CaixaAnteriorId",
                table: "Caixas");

            migrationBuilder.DropIndex(
                name: "IX_Caixas_CaixaAnteriorId",
                table: "Caixas");

            migrationBuilder.DropColumn(
                name: "CaixaAnteriorId",
                table: "Caixas");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorAberto",
                table: "Caixas",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
