using Microsoft.EntityFrameworkCore;
using SistemaGestionEmpleados.Domain.Entities;
using SistemaGestionEmpleados.Domain.Interfaces;

namespace SistemaGestionEmpleados.Infrastructure.Persistence.Repositories;

public sealed class AsignacionRepository : IAsignacionRepository
{
    private readonly GestionEmpleadosDbContext _context;

    public AsignacionRepository(GestionEmpleadosDbContext context)
    {
        _context = context;
    }

    public async Task<Asignacion?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.Asignaciones.FindAsync(new object[] { id }, cancellationToken);

    public async Task<IReadOnlyList<Asignacion>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Asignaciones
            .AsNoTracking()
            .OrderByDescending(a => a.FechaAsignacion)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Asignacion>> GetAllDetailedAsync(
        int? departamentoId = null,
        int? empleadoId = null,
        bool? soloActivas = null,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Asignaciones
            .AsNoTracking()
            .Include(a => a.Empleado)
            .Include(a => a.Departamento)
            .AsQueryable();

        if (departamentoId.HasValue)
        {
            query = query.Where(a => a.DepartamentoId == departamentoId.Value);
        }

        if (empleadoId.HasValue)
        {
            query = query.Where(a => a.EmpleadoId == empleadoId.Value);
        }

        if (soloActivas.HasValue)
        {
            query = query.Where(a => a.Activa == soloActivas.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(a =>
                a.Cargo.Contains(term) ||
                (a.Empleado != null && (a.Empleado.Nombre.Contains(term) || a.Empleado.Apellido.Contains(term) || a.Empleado.Email.Contains(term))) ||
                (a.Departamento != null && a.Departamento.Nombre.Contains(term)));
        }

        return await query
            .OrderByDescending(a => a.Activa)
            .ThenByDescending(a => a.FechaAsignacion)
            .ThenByDescending(a => a.AsignacionId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Asignacion?> GetByIdDetailedAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.Asignaciones
            .AsNoTracking()
            .Include(a => a.Empleado)
            .Include(a => a.Departamento)
            .FirstOrDefaultAsync(a => a.AsignacionId == id, cancellationToken);

    public async Task<bool> ExistsActiveDuplicateAsync(
        int empleadoId,
        int departamentoId,
        int? excludeAsignacionId = null,
        CancellationToken cancellationToken = default) =>
        await _context.Asignaciones.AnyAsync(a =>
            a.EmpleadoId == empleadoId &&
            a.DepartamentoId == departamentoId &&
            a.Activa &&
            (!excludeAsignacionId.HasValue || a.AsignacionId != excludeAsignacionId.Value),
            cancellationToken);

    public async Task ClearPrincipalForEmpleadoAsync(
        int empleadoId,
        int? excludeAsignacionId = null,
        CancellationToken cancellationToken = default)
    {
        var actuales = await _context.Asignaciones
            .Where(a => a.EmpleadoId == empleadoId &&
                        a.EsPrincipal &&
                        (!excludeAsignacionId.HasValue || a.AsignacionId != excludeAsignacionId.Value))
            .ToListAsync(cancellationToken);

        foreach (var item in actuales)
        {
            item.EsPrincipal = false;
        }
    }

    public async Task AddAsync(Asignacion entity, CancellationToken cancellationToken = default) =>
        await _context.Asignaciones.AddAsync(entity, cancellationToken);

    public void Update(Asignacion entity) =>
        _context.Asignaciones.Update(entity);

    public void Remove(Asignacion entity) =>
        _context.Asignaciones.Remove(entity);
}
