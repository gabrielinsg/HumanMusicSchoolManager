using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class RelatorioMatricula : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RelatorioMatriculas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PessoaId = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(nullable: true),
                    MatriculaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatorioMatriculas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatorioMatriculas_Matriculas_MatriculaId",
                        column: x => x.MatriculaId,
                        principalTable: "Matriculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelatorioMatriculas_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelatorioMatriculas_MatriculaId",
                table: "RelatorioMatriculas",
                column: "MatriculaId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatorioMatriculas_PessoaId",
                table: "RelatorioMatriculas",
                column: "PessoaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelatorioMatriculas");
        }
    }
}
