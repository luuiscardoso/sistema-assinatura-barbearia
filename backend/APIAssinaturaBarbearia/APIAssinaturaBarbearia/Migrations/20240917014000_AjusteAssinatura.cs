using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIAssinaturaBarbearia.Migrations
{
    /// <inheritdoc />
    public partial class AjusteAssinatura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempoRestante",
                table: "Assinaturas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "TempoRestante",
                table: "Assinaturas",
                type: "time(0)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
