using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class CandidatoCPF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Candidatos",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Candidatos");
        }
    }
}
