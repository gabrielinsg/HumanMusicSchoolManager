using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class CandidatoTelefones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cel",
                table: "Candidatos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tel",
                table: "Candidatos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cel",
                table: "Candidatos");

            migrationBuilder.DropColumn(
                name: "Tel",
                table: "Candidatos");
        }
    }
}
