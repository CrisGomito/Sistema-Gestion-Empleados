using SistemaGestionEmpleados.Domain.Entities;

namespace SistemaGestionEmpleados.Domain.Interfaces;

public interface IDepartamentoRepository : IRepository<Departamento>
{
    Task<IReadOnlyList<Departamento>> GetAllWithAsignacionesAsync(string? search = null, CancellationToken cancellationToken = default);
    Task<Departamento?> GetByIdWithAsignacionesAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsNombreAsync(string nombre, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsExtensionAsync(string extension, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> HasActiveAsignacionesAsync(int departamentoId, CancellationToken cancellationToken = default);
}
