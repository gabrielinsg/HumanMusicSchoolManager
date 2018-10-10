using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HumanMusicSchoolManager.Data.Migrations
{
    public partial class Sala : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CursoProfessor_Pessoas_AlunoId",
                table: "CursoProfessor");

            migrationBuilder.DropForeignKey(
                name: "FK_CursoProfessor_Pessoas_FuncionarioId",
                table: "CursoProfessor");

            migrationBuilder.DropIndex(
                name: "IX_CursoProfessor_AlunoId",
                table: "CursoProfessor");

            migrationBuilder.DropIndex(
                name: "IX_CursoProfessor_FuncionarioId",
                table: "CursoProfessor");

            migrationBuilder.DropColumn(
                name: "AlunoId",
                table: "CursoProfessor");

            migrationBuilder.DropColumn(
                name: "FuncionarioId",
                table: "CursoProfessor");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Pessoas",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "QtdModulo",
                table: "Cursos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Salas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ativo = table.Column<bool>(nullable: false),
                    Capacidade = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CursoSala",
                columns: table => new
                {
                    SalaId = table.Column<int>(nullable: false),
                    CursoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CursoSala", x => new { x.SalaId, x.CursoId });
                    table.ForeignKey(
                        name: "FK_CursoSala_Cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Cursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CursoSala_Salas_SalaId",
                        column: x => x.SalaId,
                        principalTable: "Salas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_CPF",
                table: "Pessoas",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CursoSala_CursoId",
                table: "CursoSala",
                column: "CursoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CursoSala");

            migrationBuilder.DropTable(
                name: "Salas");

            migrationBuilder.DropIndex(
                name: "IX_Pessoas_CPF",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "QtdModulo",
                table: "Cursos");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Pessoas",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "AlunoId",
                table: "CursoProfessor",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FuncionarioId",
                table: "CursoProfessor",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CursoProfessor_AlunoId",
                table: "CursoProfessor",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_CursoProfessor_FuncionarioId",
                table: "CursoProfessor",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_CursoProfessor_Pessoas_AlunoId",
                table: "CursoProfessor",
                column: "AlunoId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CursoProfessor_Pessoas_FuncionarioId",
                table: "CursoProfessor",
                column: "FuncionarioId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
