using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class MatriculaDispSalaNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_DispSalas_DispSalaId",
                table: "Matriculas");

            migrationBuilder.AlterColumn<int>(
                name: "DispSalaId",
                table: "Matriculas",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_DispSalas_DispSalaId",
                table: "Matriculas",
                column: "DispSalaId",
                principalTable: "DispSalas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_DispSalas_DispSalaId",
                table: "Matriculas");

            migrationBuilder.AlterColumn<int>(
                name: "DispSalaId",
                table: "Matriculas",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_DispSalas_DispSalaId",
                table: "Matriculas",
                column: "DispSalaId",
                principalTable: "DispSalas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
