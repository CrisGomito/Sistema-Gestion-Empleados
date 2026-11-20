using Microsoft.AspNetCore.Mvc;
using SistemaGestionEmpleados.Application.DTOs;
using SistemaGestionEmpleados.Application.Interfaces;

namespace SistemaGestionEmpleados.Web.Controllers;

public sealed class DepartamentosController : Controller
{
    private readonly IDepartamentoService _departamentoService;
    private readonly IAsignacionService _asignacionService;

    public DepartamentosController(
        IDepartamentoService departamentoService,
        IAsignacionService asignacionService)
    {
        _departamentoService = departamentoService;
        _asignacionService = asignacionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, CancellationToken cancellationToken)
    {
        ViewData["Search"] = search;
        var items = await _departamentoService.GetAllAsync(search, cancellationToken);
        return View(items);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var dto = await _departamentoService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();

        ViewBag.Asignaciones = await _asignacionService.GetAllAsync(departamentoId: id, cancellationToken: cancellationToken);
        return View(dto);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new DepartamentoDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartamentoDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _departamentoService.CreateAsync(dto, cancellationToken);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(result.ErrorField ?? string.Empty, result.ErrorMessage ?? "No se pudo crear el departamento.");
            return View(dto);
        }

        TempData["SuccessMessage"] = $"Departamento \"{dto.Nombre}\" creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var dto = await _departamentoService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DepartamentoDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.DepartamentoId) return BadRequest();

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _departamentoService.UpdateAsync(dto, cancellationToken);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(result.ErrorField ?? string.Empty, result.ErrorMessage ?? "No se pudo actualizar el departamento.");
            return View(dto);
        }

        TempData["SuccessMessage"] = $"Departamento \"{dto.Nombre}\" actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var dto = await _departamentoService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();

        return View(dto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var result = await _departamentoService.DeleteAsync(id, cancellationToken);
        if (!result.Succeeded)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Delete), new { id });
        }

        TempData["SuccessMessage"] = "Departamento eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
