using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class ChamadaAula : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financeiros_Pessoas_AlunoId",
                table: "Financeiros");

            migrationBuilder.DropTable(
                name: "DiariosClasse");

            migrationBuilder.AlterColumn<int>(
                name: "PessoaId",
                table: "Financeiros",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "AlunoId",
                table: "Financeiros",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "Aula",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(nullable: false),
                    ProfessorId = table.Column<int>(nullable: false),
                    CursoId = table.Column<int>(nullable: false),
                    SalaId = table.Column<int>(nullable: false),
                    AulaDada = table.Column<bool>(nullable: false),
                    DescAtividades = table.Column<string>(nullable: true),
                    ChamadaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aula", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aula_Cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Cursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aula_Pessoas_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aula_Salas_SalaId",
                        column: x => x.SalaId,
                        principalTable: "Salas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chamada",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Observacao = table.Column<string>(nullable: true),
                    PacoteCompraId = table.Column<int>(nullable: false),
                    AulaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chamada", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chamada_Aula_AulaId",
                        column: x => x.AulaId,
                        principalTable: "Aula",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chamada_PacoteCompras_PacoteCompraId",
                        column: x => x.PacoteCompraId,
                        principalTable: "PacoteCompras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aula_CursoId",
                table: "Aula",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_Aula_ProfessorId",
                table: "Aula",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Aula_SalaId",
                table: "Aula",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_Chamada_AulaId",
                table: "Chamada",
                column: "AulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Chamada_PacoteCompraId",
                table: "Chamada",
                column: "PacoteCompraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Financeiros_Pessoas_AlunoId",
                table: "Financeiros",
                column: "AlunoId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financeiros_Pessoas_AlunoId",
                table: "Financeiros");

            migrationBuilder.DropTable(
                name: "Chamada");

            migrationBuilder.DropTable(
                name: "Aula");

            migrationBuilder.AlterColumn<int>(
                name: "PessoaId",
                table: "Financeiros",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AlunoId",
                table: "Financeiros",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DiariosClasse",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(nullable: false),
                    DescAtividades = table.Column<string>(maxLength: 500, nullable: false),
                    MatriculaId = table.Column<int>(nullable: false),
                    Presenca = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiariosClasse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiariosClasse_Matriculas_MatriculaId",
                        column: x => x.MatriculaId,
                        principalTable: "Matriculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiariosClasse_MatriculaId",
                table: "DiariosClasse",
                column: "MatriculaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Financeiros_Pessoas_AlunoId",
                table: "Financeiros",
                column: "AlunoId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
