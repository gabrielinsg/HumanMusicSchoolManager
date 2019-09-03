using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class MatriculaMotivoCancelamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Motivo",
                table: "Matriculas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Outros",
                table: "Matriculas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Motivo",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "Outros",
                table: "Matriculas");
        }
    }
}
