using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Data.Migrations
{
    public partial class PacoteCompraDesconto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Desconto",
                table: "PacoteCompras",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desconto",
                table: "PacoteCompras");
        }
    }
}
