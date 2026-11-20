using Microsoft.Extensions.DependencyInjection;
using SistemaGestionEmpleados.Application.Interfaces;
using SistemaGestionEmpleados.Application.Services;

namespace SistemaGestionEmpleados.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDepartamentoService, DepartamentoService>();
        services.AddScoped<IEmpleadoService, EmpleadoService>();
        services.AddScoped<IAsignacionService, AsignacionService>();
        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }
}
