using SistemaGestionEmpleados.Domain.Entities;

namespace SistemaGestionEmpleados.Domain.Interfaces;

public interface IAsignacionRepository : IRepository<Asignacion>
{
    Task<IReadOnlyList<Asignacion>> GetAllDetailedAsync(
        int? departamentoId = null,
        int? empleadoId = null,
        bool? soloActivas = null,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<Asignacion?> GetByIdDetailedAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExistsActiveDuplicateAsync(
        int empleadoId,
        int departamentoId,
        int? excludeAsignacionId = null,
        CancellationToken cancellationToken = default);

    Task ClearPrincipalForEmpleadoAsync(
        int empleadoId,
        int? excludeAsignacionId = null,
        CancellationToken cancellationToken = default);
}
