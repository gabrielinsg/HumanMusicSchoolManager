using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HumanMusicSchoolManager.Data.Migrations
{
    public partial class MatriculaPacoteCompra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Pessoas_ProfessorId",
                table: "Matriculas");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Pessoas_RespFinanceiroId",
                table: "Matriculas");

            migrationBuilder.DropIndex(
                name: "IX_Matriculas_ProfessorId",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "Dia",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "Hora",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "Matriculas");

            migrationBuilder.AlterColumn<int>(
                name: "RespFinanceiroId",
                table: "Matriculas",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DispSalaId",
                table: "Matriculas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PacoteCompras",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DataCompra = table.Column<DateTime>(nullable: false),
                    MatriculaId = table.Column<int>(nullable: false),
                    PacoteAulaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PacoteCompras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PacoteCompras_Matriculas_MatriculaId",
                        column: x => x.MatriculaId,
                        principalTable: "Matriculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PacoteCompras_PacotesAula_PacoteAulaId",
                        column: x => x.PacoteAulaId,
                        principalTable: "PacotesAula",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_DispSalaId",
                table: "Matriculas",
                column: "DispSalaId");

            migrationBuilder.CreateIndex(
                name: "IX_PacoteCompras_MatriculaId",
                table: "PacoteCompras",
                column: "MatriculaId");

            migrationBuilder.CreateIndex(
                name: "IX_PacoteCompras_PacoteAulaId",
                table: "PacoteCompras",
                column: "PacoteAulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_DispSalas_DispSalaId",
                table: "Matriculas",
                column: "DispSalaId",
                principalTable: "DispSalas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Pessoas_RespFinanceiroId",
                table: "Matriculas",
                column: "RespFinanceiroId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_DispSalas_DispSalaId",
                table: "Matriculas");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Pessoas_RespFinanceiroId",
                table: "Matriculas");

            migrationBuilder.DropTable(
                name: "PacoteCompras");

            migrationBuilder.DropIndex(
                name: "IX_Matriculas_DispSalaId",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "DispSalaId",
                table: "Matriculas");

            migrationBuilder.AlterColumn<int>(
                name: "RespFinanceiroId",
                table: "Matriculas",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Dia",
                table: "Matriculas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Hora",
                table: "Matriculas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfessorId",
                table: "Matriculas",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Pessoas_RespFinanceiroId",
                table: "Matriculas",
                column: "RespFinanceiroId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
