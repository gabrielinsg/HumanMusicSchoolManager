using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HumanMusicSchoolManager.Data.Migrations
{
    public partial class RespFinanceiro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pessoas_CPF",
                table: "Pessoas");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Pessoas",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "RespFinanceiroId",
                table: "Matriculas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_RespFinanceiroId",
                table: "Matriculas",
                column: "RespFinanceiroId");

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
                name: "FK_Matriculas_Pessoas_RespFinanceiroId",
                table: "Matriculas");

            migrationBuilder.DropIndex(
                name: "IX_Matriculas_RespFinanceiroId",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "RespFinanceiroId",
                table: "Matriculas");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Pessoas",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_CPF",
                table: "Pessoas",
                column: "CPF",
                unique: true);
        }
    }
}
