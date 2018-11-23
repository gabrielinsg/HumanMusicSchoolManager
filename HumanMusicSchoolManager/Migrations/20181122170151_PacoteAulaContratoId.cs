using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class PacoteAulaContratoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ContratoId",
                table: "PacotesAula",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ContratoId",
                table: "PacotesAula",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
