using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIAssinaturaBarbearia.Migrations
{
    /// <inheritdoc />
    public partial class AjustandoPrecicaoTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TempoRestante",
                table: "Assinaturas",
                type: "time(0)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TempoRestante",
                table: "Assinaturas",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(0)");
        }
    }
}
