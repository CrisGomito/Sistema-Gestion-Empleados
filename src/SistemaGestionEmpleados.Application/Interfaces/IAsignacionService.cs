using SistemaGestionEmpleados.Application.Common;
using SistemaGestionEmpleados.Application.DTOs;

namespace SistemaGestionEmpleados.Application.Interfaces;

public interface IAsignacionService
{
    Task<IReadOnlyList<AsignacionDto>> GetAllAsync(
        int? departamentoId = null,
        int? empleadoId = null,
        bool? soloActivas = null,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<AsignacionDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<OperationResult<int>> CreateAsync(AsignacionDto dto, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateAsync(AsignacionDto dto, CancellationToken cancellationToken = default);
    Task<OperationResult> FinalizarAsync(int id, DateTime? fechaFin = null, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
