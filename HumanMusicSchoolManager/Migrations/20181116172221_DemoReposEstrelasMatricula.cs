using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class DemoReposEstrelasMatricula : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Estrelas",
                table: "Matriculas",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Modulo",
                table: "Matriculas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Candidatos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: false),
                    DataNasc = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidatos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reposicoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChamadaId = table.Column<int>(nullable: false),
                    DispSalaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reposicoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reposicoes_Chamadas_ChamadaId",
                        column: x => x.ChamadaId,
                        principalTable: "Chamadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reposicoes_DispSalas_DispSalaId",
                        column: x => x.DispSalaId,
                        principalTable: "DispSalas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Demostrativas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CandidatoId = table.Column<int>(nullable: false),
                    DispSalaId = table.Column<int>(nullable: false),
                    CursoId = table.Column<int>(nullable: false),
                    AulaId = table.Column<int>(nullable: false),
                    Presenca = table.Column<bool>(nullable: true),
                    Motivo = table.Column<int>(nullable: false),
                    Outros = table.Column<string>(nullable: true),
                    Observacao = table.Column<string>(nullable: true),
                    Contratou = table.Column<bool>(nullable: false),
                    Estrelas = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demostrativas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Demostrativas_Aulas_AulaId",
                        column: x => x.AulaId,
                        principalTable: "Aulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Demostrativas_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Demostrativas_Cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Cursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Demostrativas_DispSalas_DispSalaId",
                        column: x => x.DispSalaId,
                        principalTable: "DispSalas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Demostrativas_AulaId",
                table: "Demostrativas",
                column: "AulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Demostrativas_CandidatoId",
                table: "Demostrativas",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Demostrativas_CursoId",
                table: "Demostrativas",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_Demostrativas_DispSalaId",
                table: "Demostrativas",
                column: "DispSalaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reposicoes_ChamadaId",
                table: "Reposicoes",
                column: "ChamadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reposicoes_DispSalaId",
                table: "Reposicoes",
                column: "DispSalaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Demostrativas");

            migrationBuilder.DropTable(
                name: "Reposicoes");

            migrationBuilder.DropTable(
                name: "Candidatos");

            migrationBuilder.DropColumn(
                name: "Estrelas",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "Modulo",
                table: "Matriculas");
        }
    }
}
