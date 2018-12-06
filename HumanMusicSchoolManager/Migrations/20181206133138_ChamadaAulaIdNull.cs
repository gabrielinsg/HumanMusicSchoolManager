using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class ChamadaAulaIdNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chamadas_Aulas_AulaId",
                table: "Chamadas");

            migrationBuilder.AlterColumn<int>(
                name: "AulaId",
                table: "Chamadas",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Chamadas_Aulas_AulaId",
                table: "Chamadas",
                column: "AulaId",
                principalTable: "Aulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chamadas_Aulas_AulaId",
                table: "Chamadas");

            migrationBuilder.AlterColumn<int>(
                name: "AulaId",
                table: "Chamadas",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chamadas_Aulas_AulaId",
                table: "Chamadas",
                column: "AulaId",
                principalTable: "Aulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
