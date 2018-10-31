using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Data.Migrations
{
    public partial class FinanceiroTaxaMatricula : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salario",
                table: "Pessoas");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "PacotesAula",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.CreateTable(
                name: "Financeiros",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Multa = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorPago = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FormaPagamento = table.Column<int>(nullable: false),
                    DataGerada = table.Column<DateTime>(nullable: false),
                    DataVencimento = table.Column<DateTime>(nullable: false),
                    PessoaId = table.Column<int>(nullable: false),
                    AlunoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Financeiros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Financeiros_Pessoas_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Financeiros_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaxaMatriculas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxaMatriculas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Financeiros_AlunoId",
                table: "Financeiros",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_Financeiros_PessoaId",
                table: "Financeiros",
                column: "PessoaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Financeiros");

            migrationBuilder.DropTable(
                name: "TaxaMatriculas");

            migrationBuilder.AddColumn<decimal>(
                name: "Salario",
                table: "Pessoas",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "PacotesAula",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
