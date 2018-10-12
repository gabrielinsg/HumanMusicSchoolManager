using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HumanMusicSchoolManager.Data.Migrations
{
    public partial class RespFinanceiroPessoa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RespFinanceiroId",
                table: "Pessoas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_RespFinanceiroId",
                table: "Pessoas",
                column: "RespFinanceiroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pessoas_Pessoas_RespFinanceiroId",
                table: "Pessoas",
                column: "RespFinanceiroId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pessoas_Pessoas_RespFinanceiroId",
                table: "Pessoas");

            migrationBuilder.DropIndex(
                name: "IX_Pessoas_RespFinanceiroId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "RespFinanceiroId",
                table: "Pessoas");
        }
    }
}
