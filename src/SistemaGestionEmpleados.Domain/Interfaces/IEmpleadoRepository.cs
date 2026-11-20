using SistemaGestionEmpleados.Domain.Entities;

namespace SistemaGestionEmpleados.Domain.Interfaces;

public interface IEmpleadoRepository : IRepository<Empleado>
{
    Task<IReadOnlyList<Empleado>> GetAllWithAsignacionesAsync(string? search = null, CancellationToken cancellationToken = default);
    Task<Empleado?> GetByIdWithAsignacionesAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsEmailAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> HasActiveAsignacionesAsync(int empleadoId, CancellationToken cancellationToken = default);
}
