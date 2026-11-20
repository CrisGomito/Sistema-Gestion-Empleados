using SistemaGestionEmpleados.Domain.Interfaces;

namespace SistemaGestionEmpleados.Infrastructure.Persistence.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly GestionEmpleadosDbContext _context;

    public UnitOfWork(
        GestionEmpleadosDbContext context,
        IDepartamentoRepository departamentos,
        IEmpleadoRepository empleados,
        IAsignacionRepository asignaciones)
    {
        _context = context;
        Departamentos = departamentos;
        Empleados = empleados;
        Asignaciones = asignaciones;
    }

    public IDepartamentoRepository Departamentos { get; }
    public IEmpleadoRepository Empleados { get; }
    public IAsignacionRepository Asignaciones { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
