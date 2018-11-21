using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class ChamadaReposicao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reposicoes_ChamadaId",
                table: "Reposicoes");

            migrationBuilder.CreateIndex(
                name: "IX_Reposicoes_ChamadaId",
                table: "Reposicoes",
                column: "ChamadaId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reposicoes_ChamadaId",
                table: "Reposicoes");

            migrationBuilder.CreateIndex(
                name: "IX_Reposicoes_ChamadaId",
                table: "Reposicoes",
                column: "ChamadaId");
        }
    }
}
