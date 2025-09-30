using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatriculasEAD.Migrations
{
    /// <inheritdoc />
    public partial class ModelImprovements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Matriculas",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "NotaFinal",
                table: "Matriculas",
                type: "numeric(3,1)",
                precision: 3,
                scale: 1,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Data",
                table: "Matriculas",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_Titulo",
                table: "Cursos",
                column: "Titulo");

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_Email",
                table: "Alunos",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_Nome",
                table: "Alunos",
                column: "Nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cursos_Titulo",
                table: "Cursos");

            migrationBuilder.DropIndex(
                name: "IX_Alunos_Email",
                table: "Alunos");

            migrationBuilder.DropIndex(
                name: "IX_Alunos_Nome",
                table: "Alunos");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Matriculas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "NotaFinal",
                table: "Matriculas",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(3,1)",
                oldPrecision: 3,
                oldScale: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Data",
                table: "Matriculas",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
