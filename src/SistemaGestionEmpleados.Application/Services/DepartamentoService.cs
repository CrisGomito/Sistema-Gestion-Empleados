using SistemaGestionEmpleados.Application.Common;
using SistemaGestionEmpleados.Application.DTOs;
using SistemaGestionEmpleados.Application.Interfaces;
using SistemaGestionEmpleados.Domain.Entities;
using SistemaGestionEmpleados.Domain.Interfaces;

namespace SistemaGestionEmpleados.Application.Services;

/// <summary>
/// Caso de uso y reglas de negocio para la gestión de Departamentos (SRP / DIP).
/// </summary>
public sealed class DepartamentoService : IDepartamentoService
{
    private readonly IUnitOfWork _unitOfWork;

    public DepartamentoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<DepartamentoDto>> GetAllAsync(string? search = null, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.Departamentos.GetAllWithAsignacionesAsync(search, cancellationToken);
        return items.Select(MapToDto).ToList();
    }

    public async Task<DepartamentoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Departamentos.GetByIdWithAsignacionesAsync(id, cancellationToken);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<OperationResult<int>> CreateAsync(DepartamentoDto dto, CancellationToken cancellationToken = default)
    {
        var nombre = dto.Nombre.Trim();
        var extension = dto.Extension.Trim();

        if (await _unitOfWork.Departamentos.ExistsNombreAsync(nombre, null, cancellationToken))
        {
            return OperationResult<int>.Failure("Ya existe un departamento con ese nombre.", nameof(dto.Nombre));
        }

        if (await _unitOfWork.Departamentos.ExistsExtensionAsync(extension, null, cancellationToken))
        {
            return OperationResult<int>.Failure("La extensión telefónica ya está asignada a otro departamento.", nameof(dto.Extension));
        }

        var entity = new Departamento
        {
            Nombre = nombre,
            Ubicacion = dto.Ubicacion.Trim(),
            JefeDepartamento = dto.JefeDepartamento.Trim(),
            Extension = extension
        };

        await _unitOfWork.Departamentos.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OperationResult<int>.Success(entity.DepartamentoId);
    }

    public async Task<OperationResult> UpdateAsync(DepartamentoDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Departamentos.GetByIdAsync(dto.DepartamentoId, cancellationToken);
        if (entity is null)
        {
            return OperationResult.Failure("El departamento solicitado no existe.");
        }

        var nombre = dto.Nombre.Trim();
        var extension = dto.Extension.Trim();

        if (await _unitOfWork.Departamentos.ExistsNombreAsync(nombre, dto.DepartamentoId, cancellationToken))
        {
            return OperationResult.Failure("Ya existe otro departamento con ese nombre.", nameof(dto.Nombre));
        }

        if (await _unitOfWork.Departamentos.ExistsExtensionAsync(extension, dto.DepartamentoId, cancellationToken))
        {
            return OperationResult.Failure("La extensión telefónica ya pertenece a otro departamento.", nameof(dto.Extension));
        }

        entity.Nombre = nombre;
        entity.Ubicacion = dto.Ubicacion.Trim();
        entity.JefeDepartamento = dto.JefeDepartamento.Trim();
        entity.Extension = extension;

        _unitOfWork.Departamentos.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OperationResult.Success();
    }

    public async Task<OperationResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Departamentos.GetByIdWithAsignacionesAsync(id, cancellationToken);
        if (entity is null)
        {
            return OperationResult.Failure("El departamento que intenta eliminar no existe.");
        }

        if (entity.Asignaciones.Count > 0)
        {
            return OperationResult.Failure(
                "No es posible eliminar el departamento porque tiene asignaciones registradas (activas o históricas). Elimine o reasigne primero dichas asignaciones.");
        }

        _unitOfWork.Departamentos.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OperationResult.Success();
    }

    private static DepartamentoDto MapToDto(Departamento d) => new()
    {
        DepartamentoId = d.DepartamentoId,
        Nombre = d.Nombre,
        Ubicacion = d.Ubicacion,
        JefeDepartamento = d.JefeDepartamento,
        Extension = d.Extension,
        TotalAsignacionesActivas = d.Asignaciones.Count(a => a.Activa),
        TotalAsignacionesHistoricas = d.Asignaciones.Count
    };
}
