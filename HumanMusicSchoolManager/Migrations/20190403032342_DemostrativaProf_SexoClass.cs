using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class DemostrativaProf_SexoClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sexo",
                table: "Pessoas",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Dia",
                table: "Demostrativas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Hora",
                table: "Demostrativas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfessorId",
                table: "Demostrativas",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sexo",
                table: "Candidatos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Demostrativas_ProfessorId",
                table: "Demostrativas",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Demostrativas_Pessoas_ProfessorId",
                table: "Demostrativas",
                column: "ProfessorId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demostrativas_Pessoas_ProfessorId",
                table: "Demostrativas");

            migrationBuilder.DropIndex(
                name: "IX_Demostrativas_ProfessorId",
                table: "Demostrativas");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Dia",
                table: "Demostrativas");

            migrationBuilder.DropColumn(
                name: "Hora",
                table: "Demostrativas");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "Demostrativas");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Candidatos");
        }
    }
}
