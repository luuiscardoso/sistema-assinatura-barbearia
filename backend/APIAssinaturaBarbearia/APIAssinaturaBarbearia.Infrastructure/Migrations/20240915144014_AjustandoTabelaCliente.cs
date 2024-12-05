using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIAssinaturaBarbearia.Migrations
{
    /// <inheritdoc />
    public partial class AjustandoTabelaCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cpf",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Clientes");
        }
    }
}
