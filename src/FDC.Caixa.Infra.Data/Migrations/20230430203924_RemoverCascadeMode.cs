using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDC.Caixa.Infra.Data.Migrations
{
    public partial class RemoverCascadeMode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "Movimentacao");

            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "FluxoDeCaixa");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "Movimentacao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "FluxoDeCaixa",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
