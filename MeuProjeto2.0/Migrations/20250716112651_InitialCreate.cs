using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeuProjeto2._0.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Formularios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoraI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoraFim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ausente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpresaDescricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PedidoPor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pedido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gastos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TempoDeslocacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formularios", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Formularios");
        }
    }
}
