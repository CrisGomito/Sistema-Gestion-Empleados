using Microsoft.Extensions.DependencyInjection;
using SistemaGestionEmpleados.Domain.Interfaces;
using SistemaGestionEmpleados.Infrastructure.Persistence.Repositories;

namespace SistemaGestionEmpleados.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registra los repositorios concretos y Unit of Work respetando el principio de Inversión de Dependencias (DIP).
    /// El DbContext se registra explícitamente en Program.cs con la cadena de conexión "cn".
    /// </summary>
    public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
        services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
        services.AddScoped<IAsignacionRepository, AsignacionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
