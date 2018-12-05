using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class RespFinanceiroNaturalidade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Naturidade",
                table: "Pessoas",
                newName: "Naturalidade");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Naturalidade",
                table: "Pessoas",
                newName: "Naturidade");
        }
    }
}
