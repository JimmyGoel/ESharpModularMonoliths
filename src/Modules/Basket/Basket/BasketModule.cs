
using Basket.Data.Processors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Basket
{
    public static class BasketModule
    {
        public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.Decorate<IBasketRepository, CachedBasketRepositry>();
            // Add Catalog module services here

            var conectionString = configuration.GetConnectionString("Database");

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

            services.AddDbContext<BasketDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(conectionString);
            });

            // OutBox Processor Background Service
            services.AddHostedService<OutBoxProcessors>();

            return services;
        }
        public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
        {
            // Add Catalog module services here
            app.UseMigration<BasketDbContext>();

            return app;
        }
    }
}
