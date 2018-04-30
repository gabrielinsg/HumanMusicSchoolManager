using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HumanMusicSchoolManager.Data.Migrations
{
    public partial class ModEmailProfessorPessoa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlunoId",
                table: "CursoProfessor",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PessoaId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CursoProfessor_AlunoId",
                table: "CursoProfessor",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PessoaId",
                table: "AspNetUsers",
                column: "PessoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Pessoas_PessoaId",
                table: "AspNetUsers",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CursoProfessor_Pessoas_AlunoId",
                table: "CursoProfessor",
                column: "AlunoId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Pessoas_PessoaId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CursoProfessor_Pessoas_AlunoId",
                table: "CursoProfessor");

            migrationBuilder.DropIndex(
                name: "IX_CursoProfessor_AlunoId",
                table: "CursoProfessor");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PessoaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AlunoId",
                table: "CursoProfessor");

            migrationBuilder.DropColumn(
                name: "PessoaId",
                table: "AspNetUsers");
        }
    }
}
