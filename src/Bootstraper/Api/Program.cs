
using Keycloak.AuthServices.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.
var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;
var orderingAssembly = typeof(OrderingModule).Assembly;

// Common Services
builder.Services.AddCarterWithAssemblies(
   catalogAssembly, basketAssembly, orderingAssembly
    );

builder.Services.AddMediatRWithAssemblies(
    catalogAssembly,
    basketAssembly, orderingAssembly);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddMassTransitWithRabbitMq(
    builder.Configuration,
    catalogAssembly,
    basketAssembly
    );

//builder.Services.AddValidatorsFromAssemblies([catalogAssembly, basketAssembly]);

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
// module Services

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// configure the HTTP request pipeline.
app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(option => { });

app.UseCatalogModule()
   .UseBasketModule()
   .UseOrderingModule();


app.Run();
