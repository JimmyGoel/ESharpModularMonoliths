using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering
{
    public static class OrderingModule
    {
        public static IServiceCollection AddOrderingModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Catalog module services here
            return services;
        }

        public static IApplicationBuilder UseOrderingModule(this IApplicationBuilder app)
        {
            // Add Catalog module services here
            return app;
        }
    }

}
