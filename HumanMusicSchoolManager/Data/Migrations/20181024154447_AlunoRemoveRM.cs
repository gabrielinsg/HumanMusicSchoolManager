using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HumanMusicSchoolManager.Data.Migrations
{
    public partial class AlunoRemoveRM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RM",
                table: "Pessoas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RM",
                table: "Pessoas",
                nullable: true);
        }
    }
}
