using Microsoft.EntityFrameworkCore;
using SistemaGestionEmpleados.Domain.Entities;
using SistemaGestionEmpleados.Domain.Interfaces;

namespace SistemaGestionEmpleados.Infrastructure.Persistence.Repositories;

public sealed class EmpleadoRepository : IEmpleadoRepository
{
    private readonly GestionEmpleadosDbContext _context;

    public EmpleadoRepository(GestionEmpleadosDbContext context)
    {
        _context = context;
    }

    public async Task<Empleado?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.Empleados.FindAsync(new object[] { id }, cancellationToken);

    public async Task<IReadOnlyList<Empleado>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Empleados
            .AsNoTracking()
            .OrderBy(e => e.Apellido)
            .ThenBy(e => e.Nombre)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Empleado>> GetAllWithAsignacionesAsync(
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Empleados
            .AsNoTracking()
            .Include(e => e.Asignaciones)
                .ThenInclude(a => a.Departamento)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(e =>
                e.Nombre.Contains(term) ||
                e.Apellido.Contains(term) ||
                e.Email.Contains(term) ||
                e.Telefono.Contains(term));
        }

        return await query
            .OrderBy(e => e.Apellido)
            .ThenBy(e => e.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<Empleado?> GetByIdWithAsignacionesAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.Empleados
            .Include(e => e.Asignaciones)
                .ThenInclude(a => a.Departamento)
            .FirstOrDefaultAsync(e => e.EmpleadoId == id, cancellationToken);

    public async Task<bool> ExistsEmailAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default) =>
        await _context.Empleados
            .AnyAsync(e => e.Email == email && (!excludeId.HasValue || e.EmpleadoId != excludeId.Value), cancellationToken);

    public async Task<bool> HasActiveAsignacionesAsync(int empleadoId, CancellationToken cancellationToken = default) =>
        await _context.Asignaciones
            .AnyAsync(a => a.EmpleadoId == empleadoId && a.Activa, cancellationToken);

    public async Task AddAsync(Empleado entity, CancellationToken cancellationToken = default) =>
        await _context.Empleados.AddAsync(entity, cancellationToken);

    public void Update(Empleado entity) =>
        _context.Empleados.Update(entity);

    public void Remove(Empleado entity) =>
        _context.Empleados.Remove(entity);
}
