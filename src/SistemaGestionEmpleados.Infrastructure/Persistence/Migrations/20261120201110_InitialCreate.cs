using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaGestionEmpleados.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    departamento_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ubicacion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    jefe_departamento = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    extension = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.departamento_id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    empleado_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    apellido = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    telefono = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.empleado_id);
                });

            migrationBuilder.CreateTable(
                name: "Asignaciones",
                columns: table => new
                {
                    asignacion_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    empleado_id = table.Column<int>(type: "int", nullable: false),
                    departamento_id = table.Column<int>(type: "int", nullable: false),
                    cargo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    fecha_asignacion = table.Column<DateTime>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "date", nullable: true),
                    es_principal = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    activa = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    observaciones = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignaciones", x => x.asignacion_id);
                    table.CheckConstraint("CK_Asignaciones_Fechas", "[fecha_fin] IS NULL OR [fecha_fin] >= [fecha_asignacion]");
                    table.ForeignKey(
                        name: "FK_Asignaciones_Departamentos",
                        column: x => x.departamento_id,
                        principalTable: "Departamentos",
                        principalColumn: "departamento_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Asignaciones_Empleados",
                        column: x => x.empleado_id,
                        principalTable: "Empleados",
                        principalColumn: "empleado_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asignaciones_departamento_id",
                table: "Asignaciones",
                column: "departamento_id");

            migrationBuilder.CreateIndex(
                name: "UX_Asignaciones_Activa_Empleado_Departamento",
                table: "Asignaciones",
                columns: new[] { "empleado_id", "departamento_id" },
                unique: true,
                filter: "[activa] = 1");

            migrationBuilder.CreateIndex(
                name: "UX_Departamentos_Extension",
                table: "Departamentos",
                column: "extension",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Departamentos_Nombre",
                table: "Departamentos",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_Apellido_Nombre",
                table: "Empleados",
                columns: new[] { "apellido", "nombre" });

            migrationBuilder.CreateIndex(
                name: "UX_Empleados_Email",
                table: "Empleados",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asignaciones");

            migrationBuilder.DropTable(
                name: "Departamentos");

            migrationBuilder.DropTable(
                name: "Empleados");
        }
    }
}
