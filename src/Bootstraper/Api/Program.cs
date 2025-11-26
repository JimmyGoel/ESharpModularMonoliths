
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.
var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;

// Common Services
builder.Services.AddCarterWithAssemblies(
   catalogAssembly, basketAssembly
    );

builder.Services.AddMediatRWithAssemblies(
    catalogAssembly,
    basketAssembly);
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
