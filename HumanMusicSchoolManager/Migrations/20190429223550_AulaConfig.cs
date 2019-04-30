using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanMusicSchoolManager.Migrations
{
    public partial class AulaConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DuracaoAula",
                table: "Cursos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AulaConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TempoLimiteLancamento = table.Column<int>(nullable: false),
                    LancamentoAtrasado = table.Column<bool>(nullable: false),
                    MinCaracteresDescAtividades = table.Column<int>(nullable: false),
                    DescAtividadesObrigatorio = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AulaConfig", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AulaConfig");

            migrationBuilder.DropColumn(
                name: "DuracaoAula",
                table: "Cursos");
        }
    }
}
