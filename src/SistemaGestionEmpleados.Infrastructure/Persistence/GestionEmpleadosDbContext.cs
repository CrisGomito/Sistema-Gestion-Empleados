using Microsoft.EntityFrameworkCore;
using SistemaGestionEmpleados.Domain.Entities;

namespace SistemaGestionEmpleados.Infrastructure.Persistence;

/// <summary>
/// Contexto principal de Entity Framework Core 10 para SQL Server.
/// Aplica automáticamente todas las configuraciones Fluent API (IEntityTypeConfiguration).
/// </summary>
public class GestionEmpleadosDbContext : DbContext
{
    public GestionEmpleadosDbContext(DbContextOptions<GestionEmpleadosDbContext> options)
        : base(options)
    {
    }

    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<Asignacion> Asignaciones => Set<Asignacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GestionEmpleadosDbContext).Assembly);
    }
}
