using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDC.Caixa.Infra.Data.Migrations
{
    public partial class AlterarCampoDePeriodoERemoverSaldo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Saldo",
                table: "FluxoDeCaixa");

            migrationBuilder.RenameColumn(
                name: "Periodo",
                table: "Movimentacao",
                newName: "DataHora");

            migrationBuilder.AddColumn<int>(
                name: "Situacao",
                table: "FluxoDeCaixa",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Situacao",
                table: "FluxoDeCaixa");

            migrationBuilder.RenameColumn(
                name: "DataHora",
                table: "Movimentacao",
                newName: "Periodo");

            migrationBuilder.AddColumn<decimal>(
                name: "Saldo",
                table: "FluxoDeCaixa",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
