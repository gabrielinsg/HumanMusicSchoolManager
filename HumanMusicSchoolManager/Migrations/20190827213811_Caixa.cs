using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class Caixa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Caixas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DataAberto = table.Column<DateTime>(nullable: false),
                    DataFechado = table.Column<DateTime>(nullable: true),
                    FuncionarioAbertoId = table.Column<int>(nullable: false),
                    FuncionarioFechadoId = table.Column<int>(nullable: true),
                    ValorAberto = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caixas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Caixas_Pessoas_FuncionarioAbertoId",
                        column: x => x.FuncionarioAbertoId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Caixas_Pessoas_FuncionarioFechadoId",
                        column: x => x.FuncionarioFechadoId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransacoesCaixa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaixaId = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(nullable: false),
                    Valor = table.Column<decimal>(nullable: false),
                    Entrada = table.Column<bool>(nullable: false),
                    FuncionarioId = table.Column<int>(nullable: false),
                    FormaPagamento = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransacoesCaixa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransacoesCaixa_Caixas_CaixaId",
                        column: x => x.CaixaId,
                        principalTable: "Caixas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransacoesCaixa_Pessoas_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Caixas_FuncionarioAbertoId",
                table: "Caixas",
                column: "FuncionarioAbertoId");

            migrationBuilder.CreateIndex(
                name: "IX_Caixas_FuncionarioFechadoId",
                table: "Caixas",
                column: "FuncionarioFechadoId");

            migrationBuilder.CreateIndex(
                name: "IX_TransacoesCaixa_CaixaId",
                table: "TransacoesCaixa",
                column: "CaixaId");

            migrationBuilder.CreateIndex(
                name: "IX_TransacoesCaixa_FuncionarioId",
                table: "TransacoesCaixa",
                column: "FuncionarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransacoesCaixa");

            migrationBuilder.DropTable(
                name: "Caixas");
        }
    }
}
