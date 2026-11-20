using Microsoft.EntityFrameworkCore;
using SistemaGestionEmpleados.Domain.Entities;
using SistemaGestionEmpleados.Domain.Interfaces;

namespace SistemaGestionEmpleados.Infrastructure.Persistence.Repositories;

public sealed class DepartamentoRepository : IDepartamentoRepository
{
    private readonly GestionEmpleadosDbContext _context;

    public DepartamentoRepository(GestionEmpleadosDbContext context)
    {
        _context = context;
    }

    public async Task<Departamento?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.Departamentos.FindAsync(new object[] { id }, cancellationToken);

    public async Task<IReadOnlyList<Departamento>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Departamentos
            .AsNoTracking()
            .OrderBy(d => d.Nombre)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Departamento>> GetAllWithAsignacionesAsync(
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Departamentos
            .AsNoTracking()
            .Include(d => d.Asignaciones)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(d =>
                d.Nombre.Contains(term) ||
                d.Ubicacion.Contains(term) ||
                d.JefeDepartamento.Contains(term) ||
                d.Extension.Contains(term));
        }

        return await query
            .OrderBy(d => d.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<Departamento?> GetByIdWithAsignacionesAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.Departamentos
            .Include(d => d.Asignaciones)
                .ThenInclude(a => a.Empleado)
            .FirstOrDefaultAsync(d => d.DepartamentoId == id, cancellationToken);

    public async Task<bool> ExistsNombreAsync(string nombre, int? excludeId = null, CancellationToken cancellationToken = default) =>
        await _context.Departamentos
            .AnyAsync(d => d.Nombre == nombre && (!excludeId.HasValue || d.DepartamentoId != excludeId.Value), cancellationToken);

    public async Task<bool> ExistsExtensionAsync(string extension, int? excludeId = null, CancellationToken cancellationToken = default) =>
        await _context.Departamentos
            .AnyAsync(d => d.Extension == extension && (!excludeId.HasValue || d.DepartamentoId != excludeId.Value), cancellationToken);

    public async Task<bool> HasActiveAsignacionesAsync(int departamentoId, CancellationToken cancellationToken = default) =>
        await _context.Asignaciones
            .AnyAsync(a => a.DepartamentoId == departamentoId && a.Activa, cancellationToken);

    public async Task AddAsync(Departamento entity, CancellationToken cancellationToken = default) =>
        await _context.Departamentos.AddAsync(entity, cancellationToken);

    public void Update(Departamento entity) =>
        _context.Departamentos.Update(entity);

    public void Remove(Departamento entity) =>
        _context.Departamentos.Remove(entity);
}
