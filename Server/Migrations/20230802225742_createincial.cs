using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PrestigeFinancial.Server.Migrations
{
    /// <inheritdoc />
    public partial class createincial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombres = table.Column<string>(type: "TEXT", nullable: false),
                    cedula = table.Column<string>(type: "TEXT", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Direccion = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteId);
                });

            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    PrestamoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaPrestamo = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MontoSolicitado = table.Column<double>(type: "REAL", nullable: false),
                    Interes = table.Column<decimal>(type: "TEXT", nullable: false),
                    Coutas = table.Column<int>(type: "INTEGER", nullable: false),
                    Balance = table.Column<double>(type: "REAL", nullable: false),
                    TipoPrestamo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.PrestamoId);
                });

            migrationBuilder.CreateTable(
                name: "TiposPrestamos",
                columns: table => new
                {
                    TiposPrestamoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DescripcionPrestamo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposPrestamos", x => x.TiposPrestamoId);
                });

            migrationBuilder.CreateTable(
                name: "TiposTelefonos",
                columns: table => new
                {
                    TiposTelefonoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposTelefonos", x => x.TiposTelefonoId);
                });

            migrationBuilder.CreateTable(
                name: "ClientesDetalle",
                columns: table => new
                {
                    DetalleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    TiposTelefonoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Telefono = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientesDetalle", x => x.DetalleId);
                    table.ForeignKey(
                        name: "FK_ClientesDetalle_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TiposPrestamos",
                columns: new[] { "TiposPrestamoId", "DescripcionPrestamo" },
                values: new object[,]
                {
                    { 1, "Personal" },
                    { 2, "Automotriz" },
                    { 3, "Hipotecario" }
                });

            migrationBuilder.InsertData(
                table: "TiposTelefonos",
                columns: new[] { "TiposTelefonoId", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Celular" },
                    { 2, "Residencial" },
                    { 3, "Oficina" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientesDetalle_ClienteId",
                table: "ClientesDetalle",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientesDetalle");

            migrationBuilder.DropTable(
                name: "Prestamos");

            migrationBuilder.DropTable(
                name: "TiposPrestamos");

            migrationBuilder.DropTable(
                name: "TiposTelefonos");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
