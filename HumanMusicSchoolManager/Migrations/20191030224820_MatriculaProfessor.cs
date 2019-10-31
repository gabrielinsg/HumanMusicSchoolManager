using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class MatriculaProfessor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfessorId",
                table: "Matriculas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_ProfessorId",
                table: "Matriculas",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Pessoas_ProfessorId",
                table: "Matriculas",
                column: "ProfessorId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Pessoas_ProfessorId",
                table: "Matriculas");

            migrationBuilder.DropIndex(
                name: "IX_Matriculas_ProfessorId",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "Matriculas");
        }
    }
}
