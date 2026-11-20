using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SistemaGestionEmpleados.Infrastructure.Persistence;

/// <summary>
/// Fábrica en tiempo de diseño utilizada por la Consola del Administrador de Paquetes (PMC)
/// en Visual Studio Community 2026 al ejecutar Add-Migration y Update-Database.
/// </summary>
public sealed class GestionEmpleadosDbContextFactory : IDesignTimeDbContextFactory<GestionEmpleadosDbContext>
{
    public GestionEmpleadosDbContext CreateDbContext(string[] args)
    {
        var webProjectPath = Path.GetFullPath(
            Path.Combine(Directory.GetCurrentDirectory(), "..", "SistemaGestionEmpleados.Web"));

        var basePath = Directory.Exists(webProjectPath)
            ? webProjectPath
            : Directory.GetCurrentDirectory();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("cn")
            ?? "server=.\\SQLEXPRESS;pwd=12345;uid=cris;database=GestionEmpleadosDB;TrustServerCertificate=True";

        var optionsBuilder = new DbContextOptionsBuilder<GestionEmpleadosDbContext>();
        optionsBuilder.UseSqlServer(connectionString, sql =>
            sql.MigrationsAssembly(typeof(GestionEmpleadosDbContext).Assembly.FullName));

        return new GestionEmpleadosDbContext(optionsBuilder.Options);
    }
}
