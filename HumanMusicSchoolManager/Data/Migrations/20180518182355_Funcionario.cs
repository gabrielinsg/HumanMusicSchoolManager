using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HumanMusicSchoolManager.Data.Migrations
{
    public partial class Funcionario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Pessoas_PessoaId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "DescAtividades",
                table: "DiariosClasse",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "FuncionarioId",
                table: "CursoProfessor",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PessoaId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CursoProfessor_FuncionarioId",
                table: "CursoProfessor",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Pessoas_PessoaId",
                table: "AspNetUsers",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CursoProfessor_Pessoas_FuncionarioId",
                table: "CursoProfessor",
                column: "FuncionarioId",
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
                name: "FK_CursoProfessor_Pessoas_FuncionarioId",
                table: "CursoProfessor");

            migrationBuilder.DropIndex(
                name: "IX_CursoProfessor_FuncionarioId",
                table: "CursoProfessor");

            migrationBuilder.DropColumn(
                name: "FuncionarioId",
                table: "CursoProfessor");

            migrationBuilder.AlterColumn<string>(
                name: "DescAtividades",
                table: "DiariosClasse",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<int>(
                name: "PessoaId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Pessoas_PessoaId",
                table: "AspNetUsers",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
