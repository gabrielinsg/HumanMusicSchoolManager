using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class AulaChamada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aula_Cursos_CursoId",
                table: "Aula");

            migrationBuilder.DropForeignKey(
                name: "FK_Aula_Pessoas_ProfessorId",
                table: "Aula");

            migrationBuilder.DropForeignKey(
                name: "FK_Aula_Salas_SalaId",
                table: "Aula");

            migrationBuilder.DropForeignKey(
                name: "FK_Chamada_Aula_AulaId",
                table: "Chamada");

            migrationBuilder.DropForeignKey(
                name: "FK_Chamada_PacoteCompras_PacoteCompraId",
                table: "Chamada");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chamada",
                table: "Chamada");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aula",
                table: "Aula");

            migrationBuilder.DropColumn(
                name: "ChamadaId",
                table: "Aula");

            migrationBuilder.RenameTable(
                name: "Chamada",
                newName: "Chamadas");

            migrationBuilder.RenameTable(
                name: "Aula",
                newName: "Aulas");

            migrationBuilder.RenameIndex(
                name: "IX_Chamada_PacoteCompraId",
                table: "Chamadas",
                newName: "IX_Chamadas_PacoteCompraId");

            migrationBuilder.RenameIndex(
                name: "IX_Chamada_AulaId",
                table: "Chamadas",
                newName: "IX_Chamadas_AulaId");

            migrationBuilder.RenameIndex(
                name: "IX_Aula_SalaId",
                table: "Aulas",
                newName: "IX_Aulas_SalaId");

            migrationBuilder.RenameIndex(
                name: "IX_Aula_ProfessorId",
                table: "Aulas",
                newName: "IX_Aulas_ProfessorId");

            migrationBuilder.RenameIndex(
                name: "IX_Aula_CursoId",
                table: "Aulas",
                newName: "IX_Aulas_CursoId");

            migrationBuilder.AddColumn<bool>(
                name: "Presenca",
                table: "Chamadas",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chamadas",
                table: "Chamadas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aulas",
                table: "Aulas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Aulas_Cursos_CursoId",
                table: "Aulas",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Aulas_Pessoas_ProfessorId",
                table: "Aulas",
                column: "ProfessorId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Aulas_Salas_SalaId",
                table: "Aulas",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chamadas_Aulas_AulaId",
                table: "Chamadas",
                column: "AulaId",
                principalTable: "Aulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chamadas_PacoteCompras_PacoteCompraId",
                table: "Chamadas",
                column: "PacoteCompraId",
                principalTable: "PacoteCompras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aulas_Cursos_CursoId",
                table: "Aulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Aulas_Pessoas_ProfessorId",
                table: "Aulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Aulas_Salas_SalaId",
                table: "Aulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Chamadas_Aulas_AulaId",
                table: "Chamadas");

            migrationBuilder.DropForeignKey(
                name: "FK_Chamadas_PacoteCompras_PacoteCompraId",
                table: "Chamadas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chamadas",
                table: "Chamadas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aulas",
                table: "Aulas");

            migrationBuilder.DropColumn(
                name: "Presenca",
                table: "Chamadas");

            migrationBuilder.RenameTable(
                name: "Chamadas",
                newName: "Chamada");

            migrationBuilder.RenameTable(
                name: "Aulas",
                newName: "Aula");

            migrationBuilder.RenameIndex(
                name: "IX_Chamadas_PacoteCompraId",
                table: "Chamada",
                newName: "IX_Chamada_PacoteCompraId");

            migrationBuilder.RenameIndex(
                name: "IX_Chamadas_AulaId",
                table: "Chamada",
                newName: "IX_Chamada_AulaId");

            migrationBuilder.RenameIndex(
                name: "IX_Aulas_SalaId",
                table: "Aula",
                newName: "IX_Aula_SalaId");

            migrationBuilder.RenameIndex(
                name: "IX_Aulas_ProfessorId",
                table: "Aula",
                newName: "IX_Aula_ProfessorId");

            migrationBuilder.RenameIndex(
                name: "IX_Aulas_CursoId",
                table: "Aula",
                newName: "IX_Aula_CursoId");

            migrationBuilder.AddColumn<int>(
                name: "ChamadaId",
                table: "Aula",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chamada",
                table: "Chamada",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aula",
                table: "Aula",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Aula_Cursos_CursoId",
                table: "Aula",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Aula_Pessoas_ProfessorId",
                table: "Aula",
                column: "ProfessorId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Aula_Salas_SalaId",
                table: "Aula",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chamada_Aula_AulaId",
                table: "Chamada",
                column: "AulaId",
                principalTable: "Aula",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chamada_PacoteCompras_PacoteCompraId",
                table: "Chamada",
                column: "PacoteCompraId",
                principalTable: "PacoteCompras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
