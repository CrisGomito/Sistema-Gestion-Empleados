using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestionEmpleados.Application.DTOs;
using SistemaGestionEmpleados.Application.Interfaces;
using SistemaGestionEmpleados.Web.ViewModels;

namespace SistemaGestionEmpleados.Web.Controllers;

public sealed class AsignacionesController : Controller
{
    private readonly IAsignacionService _asignacionService;
    private readonly IEmpleadoService _empleadoService;
    private readonly IDepartamentoService _departamentoService;

    public AsignacionesController(
        IAsignacionService asignacionService,
        IEmpleadoService empleadoService,
        IDepartamentoService departamentoService)
    {
        _asignacionService = asignacionService;
        _empleadoService = empleadoService;
        _departamentoService = departamentoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        int? departamentoId,
        string estado = "todas",
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        bool? soloActivas = estado.ToLowerInvariant() switch
        {
            "activas" => true,
            "finalizadas" => false,
            _ => null
        };

        var items = await _asignacionService.GetAllAsync(departamentoId, null, soloActivas, search, cancellationToken);
        var departamentos = await _departamentoService.GetAllAsync(null, cancellationToken);

        var vm = new AsignacionIndexViewModel
        {
            Items = items,
            DepartamentoId = departamentoId,
            Estado = estado,
            Search = search,
            Departamentos = departamentos.Select(d => new SelectListItem
            {
                Value = d.DepartamentoId.ToString(),
                Text = d.Nombre,
                Selected = departamentoId == d.DepartamentoId
            })
        };

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var dto = await _asignacionService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();

        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? empleadoId, int? departamentoId, CancellationToken cancellationToken)
    {
        var vm = await BuildFormViewModelAsync(new AsignacionDto
        {
            EmpleadoId = empleadoId ?? 0,
            DepartamentoId = departamentoId ?? 0,
            FechaAsignacion = DateTime.Today,
            Activa = true,
            EsPrincipal = true
        }, cancellationToken);

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AsignacionFormViewModel vm, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            vm = await BuildFormViewModelAsync(vm.Asignacion, cancellationToken);
            return View(vm);
        }

        var result = await _asignacionService.CreateAsync(vm.Asignacion, cancellationToken);
        if (!result.Succeeded)
        {
            var key = string.IsNullOrEmpty(result.ErrorField)
                ? string.Empty
                : $"Asignacion.{result.ErrorField}";
            ModelState.AddModelError(key, result.ErrorMessage ?? "No se pudo registrar la asignación.");
            vm = await BuildFormViewModelAsync(vm.Asignacion, cancellationToken);
            return View(vm);
        }

        TempData["SuccessMessage"] = "Asignación registrada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var dto = await _asignacionService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();

        var vm = await BuildFormViewModelAsync(dto, cancellationToken);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AsignacionFormViewModel vm, CancellationToken cancellationToken)
    {
        if (id != vm.Asignacion.AsignacionId) return BadRequest();

        if (!ModelState.IsValid)
        {
            vm = await BuildFormViewModelAsync(vm.Asignacion, cancellationToken);
            return View(vm);
        }

        var result = await _asignacionService.UpdateAsync(vm.Asignacion, cancellationToken);
        if (!result.Succeeded)
        {
            var key = string.IsNullOrEmpty(result.ErrorField)
                ? string.Empty
                : $"Asignacion.{result.ErrorField}";
            ModelState.AddModelError(key, result.ErrorMessage ?? "No se pudo actualizar la asignación.");
            vm = await BuildFormViewModelAsync(vm.Asignacion, cancellationToken);
            return View(vm);
        }

        TempData["SuccessMessage"] = "Asignación actualizada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Finalizar(int id, CancellationToken cancellationToken)
    {
        var result = await _asignacionService.FinalizarAsync(id, DateTime.Today, cancellationToken);
        TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
            result.Succeeded ? "Asignación finalizada correctamente." : result.ErrorMessage;

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var dto = await _asignacionService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();

        return View(dto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var result = await _asignacionService.DeleteAsync(id, cancellationToken);
        TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
            result.Succeeded ? "Asignación eliminada correctamente." : result.ErrorMessage;

        return RedirectToAction(nameof(Index));
    }

    private async Task<AsignacionFormViewModel> BuildFormViewModelAsync(AsignacionDto dto, CancellationToken cancellationToken)
    {
        var empleados = await _empleadoService.GetAllAsync(null, cancellationToken);
        var departamentos = await _departamentoService.GetAllAsync(null, cancellationToken);

        return new AsignacionFormViewModel
        {
            Asignacion = dto,
            Empleados = empleados.Select(e => new SelectListItem
            {
                Value = e.EmpleadoId.ToString(),
                Text = $"{e.NombreCompleto} ({e.Email})",
                Selected = e.EmpleadoId == dto.EmpleadoId
            }),
            Departamentos = departamentos.Select(d => new SelectListItem
            {
                Value = d.DepartamentoId.ToString(),
                Text = $"{d.Nombre} — Ext. {d.Extension}",
                Selected = d.DepartamentoId == dto.DepartamentoId
            })
        };
    }
}
