using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class FinanceiroDataPagamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataPagamento",
                table: "Financeiros",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataPagamento",
                table: "Financeiros");
        }
    }
}
