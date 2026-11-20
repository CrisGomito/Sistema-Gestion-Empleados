using SistemaGestionEmpleados.Application.Common;
using SistemaGestionEmpleados.Application.DTOs;

namespace SistemaGestionEmpleados.Application.Interfaces;

public interface IEmpleadoService
{
    Task<IReadOnlyList<EmpleadoDto>> GetAllAsync(string? search = null, CancellationToken cancellationToken = default);
    Task<EmpleadoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<OperationResult<int>> CreateAsync(EmpleadoDto dto, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateAsync(EmpleadoDto dto, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
