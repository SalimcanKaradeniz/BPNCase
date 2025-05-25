using ECommercePaymentApi.Middleware;
using ECommercePaymentApi.Validators;
using HealthChecks.UI.Client;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using FluentValidation;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Persistence;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);


/*Health Checks*/

builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DbConnection")!,
        name: "SQL Server",
        failureStatus: HealthStatus.Unhealthy
    );

var healthCheckEndpoint = builder.Configuration["HealthChecksUI:HealthCheckEndpoint"];

builder.Services
    .AddHealthChecksUI(settings =>
    {
        settings.AddHealthCheckEndpoint("Ecommerce Payment API", healthCheckEndpoint);
    }).AddSqlServerStorage(connectionString: builder.Configuration.GetConnectionString("HealthCheckDb"));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ECommerce API",
        Version = "v1",
        Description = "E-Commerce Payment Integration Challenge",
        Contact = new OpenApiContact
        {
            Name = "Salimcan Karadeniz",
            Email = "salimcankaradeniz@gmail.com"
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


builder.Services.AddValidatorsFromAssemblyContaining<OrderValidator>();


var app = builder.Build();

//Health Checks
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// UI arayüzü
app.UseHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
