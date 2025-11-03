

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCarterWithAssemblies(
    typeof(CatalogModule).Assembly);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

var app = builder.Build();

// configure the HTTP request pipeline.
app.MapCarter();
app.UseCatalogModule()
   .UseBasketModule()
   .UseOrderingModule();

app.Run();
