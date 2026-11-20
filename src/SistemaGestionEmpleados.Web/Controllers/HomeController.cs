using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionEmpleados.Application.Interfaces;
using SistemaGestionEmpleados.Web.ViewModels;

namespace SistemaGestionEmpleados.Web.Controllers;

public sealed class HomeController : Controller
{
    private readonly IDashboardService _dashboardService;

    public HomeController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var resumen = await _dashboardService.GetResumenAsync(cancellationToken);
        return View(resumen);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
