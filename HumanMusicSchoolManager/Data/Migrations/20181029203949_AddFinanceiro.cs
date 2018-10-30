using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Data.Migrations
{
    public partial class AddFinanceiro : Migration
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
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Multa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorPago = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FormaPagamento = table.Column<int>(nullable: false),
                    DataGerada = table.Column<DateTime>(nullable: false),
                    DateVencimento = table.Column<DateTime>(nullable: false),
                    FuncionarioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Financeiros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Financeiros_Pessoas_FuncionarioId",
                        column: x => x.FuncionarioId,
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
                name: "IX_Financeiros_FuncionarioId",
                table: "Financeiros",
                column: "FuncionarioId");
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
