using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiGenteEnLinea.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurePersonalPC_IgnoreLegacyEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empleado",
                columns: table => new
                {
                    empleadoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    fechaRegistro = table.Column<DateTime>(type: "datetime", nullable: true),
                    fechaInicio = table.Column<DateOnly>(type: "date", nullable: true),
                    identificacion = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Apellido = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    alias = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    nacimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    estadoCivil = table.Column<int>(type: "int", nullable: true),
                    direccion = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    provincia = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    municipio = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    telefono1 = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    telefono2 = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    posicion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    salario = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    periodoPago = table.Column<int>(type: "int", nullable: true),
                    contactoEmergencia = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    telefonoEmergencia = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    contrato = table.Column<bool>(type: "bit", nullable: true),
                    remuneracionExtra1 = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    montoExtra1 = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    remuneracionExtra2 = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    montoExtra2 = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    remuneracionExtra3 = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    montoExtra3 = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    tss = table.Column<bool>(type: "bit", nullable: true),
                    diasPago = table.Column<int>(type: "int", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: true),
                    fechaSalida = table.Column<DateTime>(type: "datetime", nullable: true),
                    motivoBaja = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    prestaciones = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    foto = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleado", x => x.empleadoID);
                });

            migrationBuilder.CreateTable(
                name: "OpenAi_Config",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpenAIApiKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OpenAIApiUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenAi_Config", x => x.id);
                },
                comment: "Configuración del bot OpenAI para el 'abogado virtual'. ⚠️ Contiene información sensible.");

            migrationBuilder.CreateTable(
                name: "Remuneraciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    empleadoID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remuneraciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_Remuneraciones_Empleados",
                        column: x => x.empleadoID,
                        principalTable: "Empleado",
                        principalColumn: "empleadoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Remuneraciones_EmpleadoId",
                table: "Remuneraciones",
                column: "empleadoID");

            migrationBuilder.CreateIndex(
                name: "IX_Remuneraciones_UserId",
                table: "Remuneraciones",
                column: "userID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpenAi_Config");

            migrationBuilder.DropTable(
                name: "Remuneraciones");

            migrationBuilder.DropTable(
                name: "Empleado");
        }
    }
}
