using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class CandidatoDataNascimento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataNasc",
                table: "Candidatos",
                newName: "DataNascimento");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataNascimento",
                table: "Candidatos",
                newName: "DataNasc");
        }
    }
}
