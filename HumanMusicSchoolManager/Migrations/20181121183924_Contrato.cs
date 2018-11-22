using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class Contrato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContratoId",
                table: "PacotesAula",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    Conteudo = table.Column<string>(nullable: true),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PacotesAula_ContratoId",
                table: "PacotesAula",
                column: "ContratoId");

            migrationBuilder.AddForeignKey(
                name: "FK_PacotesAula_Contratos_ContratoId",
                table: "PacotesAula",
                column: "ContratoId",
                principalTable: "Contratos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PacotesAula_Contratos_ContratoId",
                table: "PacotesAula");

            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.DropIndex(
                name: "IX_PacotesAula_ContratoId",
                table: "PacotesAula");

            migrationBuilder.DropColumn(
                name: "ContratoId",
                table: "PacotesAula");
        }
    }
}
