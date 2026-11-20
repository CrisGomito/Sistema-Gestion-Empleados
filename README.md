# Sistema de Gestión de Empleados (.NET 10 — Clean Architecture MVC)

Aplicación empresarial desarrollada estrictamente en **.NET 10 (`net10.0`)** con **ASP.NET Core MVC**, **Entity Framework Core 10**, **SQL Server**, **Clean Architecture**, principios **SOLID** y vistas Razor (`.cshtml`) estilizadas con **Tailwind CSS vía CDN**.

## Requisitos de entorno

- **Visual Studio Community 2026** con la carga de trabajo *Desarrollo de ASP.NET y web*
- **.NET 10 SDK**
- **SQL Server Express** (`.\SQLEXPRESS`) o **SQL Server LocalDB**

## Estructura de la solución (`SistemaGestionEmpleados.slnx`)

```text
SistemaGestionEmpleados.slnx
├── Directory.Build.props
├── README.md
├── docs/
│   ├── ARQUITECTURA.md
│   ├── GUIA_MIGRACION_VS2026.md
│   └── MODELO_DATOS.md
├── scripts/
│   ├── 01_PMC_Migracion_VS2026.ps1
│   ├── 02_Crear_BD_Y_Tablas_SQLServer.sql
│   └── 03_Datos_Semilla_SQLServer.sql
└── src/
    ├── SistemaGestionEmpleados.Domain/          # Entidades con Data Annotations + Contratos
    ├── SistemaGestionEmpleados.Application/     # DTOs, Interfaces y Servicios de casos de uso
    ├── SistemaGestionEmpleados.Infrastructure/  # DbContext EF Core 10, Fluent API, Repositorios y UnitOfWork
    └── SistemaGestionEmpleados.Web/             # MVC Controllers, ViewModels, Views .cshtml y wwwroot
```

## Inicio rápido en Visual Studio Community 2026

1. Abra `SistemaGestionEmpleados.slnx` en Visual Studio Community 2026.
2. Verifique la cadena de conexión `"cn"` en `src/SistemaGestionEmpleados.Web/appsettings.json`.
3. Establezca **`SistemaGestionEmpleados.Web`** como proyecto de inicio.
4. Compile la solución (`Ctrl + Shift + B`) para restaurar los paquetes NuGet `10.0.*`.
5. Ejecute las migraciones desde la **Consola del Administrador de Paquetes (PMC)** siguiendo `docs/GUIA_MIGRACION_VS2026.md`.
6. Opcionalmente ejecute `scripts/03_Datos_Semilla_SQLServer.sql` en SSMS para cargar datos de prueba.
7. Presione **F5** para ejecutar el sistema.
