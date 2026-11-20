using SistemaGestionEmpleados.Application.Common;
using SistemaGestionEmpleados.Application.DTOs;
using SistemaGestionEmpleados.Application.Interfaces;
using SistemaGestionEmpleados.Domain.Entities;
using SistemaGestionEmpleados.Domain.Interfaces;

namespace SistemaGestionEmpleados.Application.Services;

/// <summary>
/// Caso de uso para gestionar asignaciones de empleados a departamentos.
/// Garantiza integridad de departamento principal y evita duplicados activos.
/// </summary>
public sealed class AsignacionService : IAsignacionService
{
    private readonly IUnitOfWork _unitOfWork;

    public AsignacionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<AsignacionDto>> GetAllAsync(
        int? departamentoId = null,
        int? empleadoId = null,
        bool? soloActivas = null,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.Asignaciones.GetAllDetailedAsync(
            departamentoId, empleadoId, soloActivas, search, cancellationToken);

        return items.Select(MapToDto).ToList();
    }

    public async Task<AsignacionDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Asignaciones.GetByIdDetailedAsync(id, cancellationToken);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<OperationResult<int>> CreateAsync(AsignacionDto dto, CancellationToken cancellationToken = default)
    {
        var empleado = await _unitOfWork.Empleados.GetByIdAsync(dto.EmpleadoId, cancellationToken);
        if (empleado is null)
        {
            return OperationResult<int>.Failure("El empleado seleccionado no existe.", nameof(dto.EmpleadoId));
        }

        var departamento = await _unitOfWork.Departamentos.GetByIdAsync(dto.DepartamentoId, cancellationToken);
        if (departamento is null)
        {
            return OperationResult<int>.Failure("El departamento seleccionado no existe.", nameof(dto.DepartamentoId));
        }

        if (dto.Activa && await _unitOfWork.Asignaciones.ExistsActiveDuplicateAsync(dto.EmpleadoId, dto.DepartamentoId, null, cancellationToken))
        {
            return OperationResult<int>.Failure(
                "El empleado ya tiene una asignación activa en este departamento.",
                nameof(dto.DepartamentoId));
        }

        if (dto.Activa && dto.EsPrincipal)
        {
            await _unitOfWork.Asignaciones.ClearPrincipalForEmpleadoAsync(dto.EmpleadoId, null, cancellationToken);
        }

        var entity = new Asignacion
        {
            EmpleadoId = dto.EmpleadoId,
            DepartamentoId = dto.DepartamentoId,
            Cargo = dto.Cargo.Trim(),
            FechaAsignacion = dto.FechaAsignacion.Date,
            FechaFin = dto.Activa ? null : (dto.FechaFin?.Date ?? DateTime.Today),
            EsPrincipal = dto.Activa && dto.EsPrincipal,
            Activa = dto.Activa,
            Observaciones = string.IsNullOrWhiteSpace(dto.Observaciones) ? null : dto.Observaciones.Trim()
        };

        await _unitOfWork.Asignaciones.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OperationResult<int>.Success(entity.AsignacionId);
    }

    public async Task<OperationResult> UpdateAsync(AsignacionDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Asignaciones.GetByIdAsync(dto.AsignacionId, cancellationToken);
        if (entity is null)
        {
            return OperationResult.Failure("La asignación solicitada no existe.");
        }

        if (dto.Activa && await _unitOfWork.Asignaciones.ExistsActiveDuplicateAsync(dto.EmpleadoId, dto.DepartamentoId, dto.AsignacionId, cancellationToken))
        {
            return OperationResult.Failure(
                "Ya existe otra asignación activa para este empleado en el departamento seleccionado.",
                nameof(dto.DepartamentoId));
        }

        if (dto.Activa && dto.EsPrincipal)
        {
            await _unitOfWork.Asignaciones.ClearPrincipalForEmpleadoAsync(dto.EmpleadoId, dto.AsignacionId, cancellationToken);
        }

        entity.EmpleadoId = dto.EmpleadoId;
        entity.DepartamentoId = dto.DepartamentoId;
        entity.Cargo = dto.Cargo.Trim();
        entity.FechaAsignacion = dto.FechaAsignacion.Date;
        entity.Activa = dto.Activa;
        entity.FechaFin = dto.Activa ? null : (dto.FechaFin?.Date ?? DateTime.Today);
        entity.EsPrincipal = dto.Activa && dto.EsPrincipal;
        entity.Observaciones = string.IsNullOrWhiteSpace(dto.Observaciones) ? null : dto.Observaciones.Trim();

        _unitOfWork.Asignaciones.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OperationResult.Success();
    }

    public async Task<OperationResult> FinalizarAsync(int id, DateTime? fechaFin = null, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Asignaciones.GetByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            return OperationResult.Failure("La asignación solicitada no existe.");
        }

        if (!entity.Activa)
        {
            return OperationResult.Failure("La asignación ya se encuentra finalizada.");
        }

        var cierre = (fechaFin ?? DateTime.Today).Date;
        if (cierre < entity.FechaAsignacion.Date)
        {
            cierre = entity.FechaAsignacion.Date;
        }

        entity.Activa = false;
        entity.EsPrincipal = false;
        entity.FechaFin = cierre;

        _unitOfWork.Asignaciones.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OperationResult.Success();
    }

    public async Task<OperationResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Asignaciones.GetByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            return OperationResult.Failure("La asignación solicitada no existe.");
        }

        _unitOfWork.Asignaciones.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OperationResult.Success();
    }

    private static AsignacionDto MapToDto(Asignacion a) => new()
    {
        AsignacionId = a.AsignacionId,
        EmpleadoId = a.EmpleadoId,
        DepartamentoId = a.DepartamentoId,
        Cargo = a.Cargo,
        FechaAsignacion = a.FechaAsignacion,
        FechaFin = a.FechaFin,
        EsPrincipal = a.EsPrincipal,
        Activa = a.Activa,
        Observaciones = a.Observaciones,
        EmpleadoNombreCompleto = a.Empleado?.NombreCompleto ?? string.Empty,
        EmpleadoEmail = a.Empleado?.Email ?? string.Empty,
        DepartamentoNombre = a.Departamento?.Nombre ?? string.Empty,
        DepartamentoUbicacion = a.Departamento?.Ubicacion ?? string.Empty,
        DepartamentoExtension = a.Departamento?.Extension ?? string.Empty
    };
}
