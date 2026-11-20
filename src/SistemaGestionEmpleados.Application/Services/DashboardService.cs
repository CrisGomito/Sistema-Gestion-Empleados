using SistemaGestionEmpleados.Application.DTOs;
using SistemaGestionEmpleados.Application.Interfaces;
using SistemaGestionEmpleados.Domain.Interfaces;

namespace SistemaGestionEmpleados.Application.Services;

public sealed class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DashboardResumenDto> GetResumenAsync(CancellationToken cancellationToken = default)
    {
        var departamentos = await _unitOfWork.Departamentos.GetAllWithAsignacionesAsync(null, cancellationToken);
        var empleados = await _unitOfWork.Empleados.GetAllWithAsignacionesAsync(null, cancellationToken);
        var asignaciones = await _unitOfWork.Asignaciones.GetAllDetailedAsync(null, null, null, null, cancellationToken);

        var distribucion = departamentos
            .Select(d => new DepartamentoCargaDto
            {
                DepartamentoId = d.DepartamentoId,
                Nombre = d.Nombre,
                JefeDepartamento = d.JefeDepartamento,
                Extension = d.Extension,
                EmpleadosActivos = d.Asignaciones.Count(a => a.Activa)
            })
            .OrderByDescending(x => x.EmpleadosActivos)
            .ThenBy(x => x.Nombre)
            .ToList();

        var recientes = asignaciones
            .OrderByDescending(a => a.FechaAsignacion)
            .ThenByDescending(a => a.AsignacionId)
            .Take(8)
            .Select(a => new AsignacionDto
            {
                AsignacionId = a.AsignacionId,
                EmpleadoId = a.EmpleadoId,
                DepartamentoId = a.DepartamentoId,
                Cargo = a.Cargo,
                FechaAsignacion = a.FechaAsignacion,
                FechaFin = a.FechaFin,
                EsPrincipal = a.EsPrincipal,
                Activa = a.Activa,
                EmpleadoNombreCompleto = a.Empleado?.NombreCompleto ?? string.Empty,
                EmpleadoEmail = a.Empleado?.Email ?? string.Empty,
                DepartamentoNombre = a.Departamento?.Nombre ?? string.Empty,
                DepartamentoUbicacion = a.Departamento?.Ubicacion ?? string.Empty,
                DepartamentoExtension = a.Departamento?.Extension ?? string.Empty
            })
            .ToList();

        return new DashboardResumenDto
        {
            TotalDepartamentos = departamentos.Count,
            TotalEmpleados = empleados.Count,
            TotalAsignacionesActivas = asignaciones.Count(a => a.Activa),
            EmpleadosSinAsignacion = empleados.Count(e => !e.Asignaciones.Any(a => a.Activa)),
            DistribucionDepartamentos = distribucion,
            AsignacionesRecientes = recientes
        };
    }
}
