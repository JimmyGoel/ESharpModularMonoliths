
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog
{
    public static class CatalogModule
    {
        public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Service to the container.

            // Api Endpoint Services

            // Applcation Use Case Services.

            // Data - Infrastructure Services

            var conectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<CatalogDbContext>(options =>
            {
                options.UseNpgsql(conectionString);
            });

            return services;
        }

        public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
        {
            // Add Catalog module services here
            return app;
        }
    }
}
