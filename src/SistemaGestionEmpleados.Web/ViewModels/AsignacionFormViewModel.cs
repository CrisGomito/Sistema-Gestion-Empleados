using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestionEmpleados.Application.DTOs;

namespace SistemaGestionEmpleados.Web.ViewModels;

public sealed class AsignacionFormViewModel
{
    public AsignacionDto Asignacion { get; set; } = new();
    public IEnumerable<SelectListItem> Empleados { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Departamentos { get; set; } = Enumerable.Empty<SelectListItem>();
}
