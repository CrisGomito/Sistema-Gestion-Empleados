using SistemaGestionEmpleados.Application.Common;
using SistemaGestionEmpleados.Application.DTOs;

namespace SistemaGestionEmpleados.Application.Interfaces;

public interface IDepartamentoService
{
    Task<IReadOnlyList<DepartamentoDto>> GetAllAsync(string? search = null, CancellationToken cancellationToken = default);
    Task<DepartamentoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<OperationResult<int>> CreateAsync(DepartamentoDto dto, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateAsync(DepartamentoDto dto, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
