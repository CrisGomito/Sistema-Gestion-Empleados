using Microsoft.AspNetCore.Mvc;
using SistemaGestionEmpleados.Application.DTOs;
using SistemaGestionEmpleados.Application.Interfaces;

namespace SistemaGestionEmpleados.Web.Controllers;

public sealed class EmpleadosController : Controller
{
    private readonly IEmpleadoService _empleadoService;
    private readonly IAsignacionService _asignacionService;

    public EmpleadosController(
        IEmpleadoService empleadoService,
        IAsignacionService asignacionService)
    {
        _empleadoService = empleadoService;
        _asignacionService = asignacionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, CancellationToken cancellationToken)
    {
        ViewData["Search"] = search;
        var items = await _empleadoService.GetAllAsync(search, cancellationToken);
        return View(items);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var dto = await _empleadoService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();

        ViewBag.Asignaciones = await _asignacionService.GetAllAsync(empleadoId: id, cancellationToken: cancellationToken);
        return View(dto);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new EmpleadoDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmpleadoDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _empleadoService.CreateAsync(dto, cancellationToken);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(result.ErrorField ?? string.Empty, result.ErrorMessage ?? "No se pudo registrar el empleado.");
            return View(dto);
        }

        TempData["SuccessMessage"] = $"Empleado \"{dto.NombreCompleto}\" registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var dto = await _empleadoService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EmpleadoDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.EmpleadoId) return BadRequest();

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _empleadoService.UpdateAsync(dto, cancellationToken);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(result.ErrorField ?? string.Empty, result.ErrorMessage ?? "No se pudo actualizar el empleado.");
            return View(dto);
        }

        TempData["SuccessMessage"] = $"Datos de \"{dto.NombreCompleto}\" actualizados correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var dto = await _empleadoService.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();

        return View(dto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var result = await _empleadoService.DeleteAsync(id, cancellationToken);
        if (!result.Succeeded)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Delete), new { id });
        }

        TempData["SuccessMessage"] = "Empleado eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
