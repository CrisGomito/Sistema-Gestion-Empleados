using SistemaGestionEmpleados.Application.Common;
using SistemaGestionEmpleados.Application.DTOs;
using SistemaGestionEmpleados.Application.Interfaces;
using SistemaGestionEmpleados.Domain.Entities;
using SistemaGestionEmpleados.Domain.Interfaces;

namespace SistemaGestionEmpleados.Application.Services;

/// <summary>
/// Caso de uso y reglas de negocio para la gestión de Empleados.
/// </summary>
public sealed class EmpleadoService : IEmpleadoService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmpleadoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<EmpleadoDto>> GetAllAsync(string? search = null, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.Empleados.GetAllWithAsignacionesAsync(search, cancellationToken);
        return items.Select(MapToDto).ToList();
    }

    public async Task<EmpleadoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Empleados.GetByIdWithAsignacionesAsync(id, cancellationToken);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<OperationResult<int>> CreateAsync(EmpleadoDto dto, CancellationToken cancellationToken = default)
    {
        var emailNormalizado = dto.Email.Trim().ToLowerInvariant();

        if (await _unitOfWork.Empleados.ExistsEmailAsync(emailNormalizado, null, cancellationToken))
        {
            return OperationResult<int>.Failure("Ya existe un empleado registrado con ese correo electrónico.", nameof(dto.Email));
        }

        var entity = new Empleado
        {
            Nombre = dto.Nombre.Trim(),
            Apellido = dto.Apellido.Trim(),
            Email = emailNormalizado,
            Telefono = dto.Telefono.Trim()
        };

        await _unitOfWork.Empleados.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OperationResult<int>.Success(entity.EmpleadoId);
    }

    public async Task<OperationResult> UpdateAsync(EmpleadoDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Empleados.GetByIdAsync(dto.EmpleadoId, cancellationToken);
        if (entity is null)
        {
            return OperationResult.Failure("El empleado solicitado no existe.");
        }

        var emailNormalizado = dto.Email.Trim().ToLowerInvariant();
        if (await _unitOfWork.Empleados.ExistsEmailAsync(emailNormalizado, dto.EmpleadoId, cancellationToken))
        {
            return OperationResult.Failure("Otro empleado ya utiliza ese correo electrónico.", nameof(dto.Email));
        }

        entity.Nombre = dto.Nombre.Trim();
        entity.Apellido = dto.Apellido.Trim();
        entity.Email = emailNormalizado;
        entity.Telefono = dto.Telefono.Trim();

        _unitOfWork.Empleados.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OperationResult.Success();
    }

    public async Task<OperationResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Empleados.GetByIdWithAsignacionesAsync(id, cancellationToken);
        if (entity is null)
        {
            return OperationResult.Failure("El empleado que intenta eliminar no existe.");
        }

        if (entity.Asignaciones.Any(a => a.Activa))
        {
            return OperationResult.Failure(
                "No se puede eliminar un empleado que posee asignaciones activas. Finalice o elimine sus asignaciones primero.");
        }

        _unitOfWork.Empleados.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OperationResult.Success();
    }

    private static EmpleadoDto MapToDto(Empleado e)
    {
        var activas = e.Asignaciones.Where(a => a.Activa).ToList();
        var principal = activas.FirstOrDefault(a => a.EsPrincipal)?.Departamento?.Nombre
                        ?? activas.FirstOrDefault()?.Departamento?.Nombre;

        return new EmpleadoDto
        {
            EmpleadoId = e.EmpleadoId,
            Nombre = e.Nombre,
            Apellido = e.Apellido,
            Email = e.Email,
            Telefono = e.Telefono,
            DepartamentoPrincipal = principal,
            TotalAsignacionesActivas = activas.Count
        };
    }
}
