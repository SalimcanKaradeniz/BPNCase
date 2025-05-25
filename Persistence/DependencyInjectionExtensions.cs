using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repository;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Persistence;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, 
                sqlOptions =>
                {
                    sqlOptions.CommandTimeout(60);
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(15),
                        errorNumbersToAdd: [40613, 40197, 40501, 49918, 49919, 49920]);
                }
            ));

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        
        return services;
    }
}