using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class DemonstrativaFuncionario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FuncionarioId",
                table: "Demostrativas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Demostrativas_FuncionarioId",
                table: "Demostrativas",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Demostrativas_Pessoas_FuncionarioId",
                table: "Demostrativas",
                column: "FuncionarioId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demostrativas_Pessoas_FuncionarioId",
                table: "Demostrativas");

            migrationBuilder.DropIndex(
                name: "IX_Demostrativas_FuncionarioId",
                table: "Demostrativas");

            migrationBuilder.DropColumn(
                name: "FuncionarioId",
                table: "Demostrativas");
        }
    }
}
