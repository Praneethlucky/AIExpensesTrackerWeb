using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Persistence;
using ExpenseTracker.Infrastructure.AI;
using ExpenseTracker.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(
            configuration.GetSection("ConnectionStrings"));

        services.Configure<GeminiSettings>(
            configuration.GetSection("GeminiSettings"));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBillRepository, BillRepository>();
        services.AddHttpClient<IAIProvider, GeminiService>();

        return services;
    }
}