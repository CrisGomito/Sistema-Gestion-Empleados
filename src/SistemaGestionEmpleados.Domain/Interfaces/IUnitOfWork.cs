namespace SistemaGestionEmpleados.Domain.Interfaces;

/// <summary>
/// Patrón Unit of Work para coordinar transacciones atómicas entre repositorios.
/// </summary>
public interface IUnitOfWork
{
    IDepartamentoRepository Departamentos { get; }
    IEmpleadoRepository Empleados { get; }
    IAsignacionRepository Asignaciones { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
