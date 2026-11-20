namespace SistemaGestionEmpleados.Application.DTOs;

public sealed class DashboardResumenDto
{
    public int TotalDepartamentos { get; set; }
    public int TotalEmpleados { get; set; }
    public int TotalAsignacionesActivas { get; set; }
    public int EmpleadosSinAsignacion { get; set; }
    public IReadOnlyList<DepartamentoCargaDto> DistribucionDepartamentos { get; set; } = Array.Empty<DepartamentoCargaDto>();
    public IReadOnlyList<AsignacionDto> AsignacionesRecientes { get; set; } = Array.Empty<AsignacionDto>();
}

public sealed class DepartamentoCargaDto
{
    public int DepartamentoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string JefeDepartamento { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public int EmpleadosActivos { get; set; }
}
