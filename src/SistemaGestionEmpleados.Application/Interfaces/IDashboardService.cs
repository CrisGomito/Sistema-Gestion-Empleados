using SistemaGestionEmpleados.Application.DTOs;

namespace SistemaGestionEmpleados.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardResumenDto> GetResumenAsync(CancellationToken cancellationToken = default);
}
