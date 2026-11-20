using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestionEmpleados.Application.DTOs;

namespace SistemaGestionEmpleados.Web.ViewModels;

public sealed class AsignacionIndexViewModel
{
    public IReadOnlyList<AsignacionDto> Items { get; set; } = Array.Empty<AsignacionDto>();
    public int? DepartamentoId { get; set; }
    public string Estado { get; set; } = "todas";
    public string? Search { get; set; }
    public IEnumerable<SelectListItem> Departamentos { get; set; } = Enumerable.Empty<SelectListItem>();
}
