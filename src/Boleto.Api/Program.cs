using System.Reflection;
using Boleto.Api.Middleware;
using Boleto.Api.Policy;
using Boleto.Domain.Intefaces.Proxy;
using Boleto.Domain.Intefaces.Repositories;
using Boleto.Domain.Intefaces.Services;
using Boleto.Infrastructure.Repositories;
using Boleto.Service.ExternalServices;
using Boleto.Service.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace Boleto.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Adiciona os serviços de HealthCheck e verifica a API externa
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddCheck<ExternalApiHealthCheck>("external_api");

        // Adiciona HttpClient para a verificação de API externa
        builder.Services.AddHttpClient<ExternalApiHealthCheck>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Boleto API", Version = "v1" });

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
        builder.Services.AddMvc().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
        });

        builder.Services.AddScoped<IBoletoService, BoletoService>();
        builder.Services.AddScoped<IBoletoRepository, BoletoRepository>();
        builder.Services.AddScoped<IBoletoApiClient, BoletoApiClient>();

        builder.Services.AddHttpClient<BoletoApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://vagas.builders/api/builders/");
        });

        builder.Services.AddMemoryCache();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Boleto API V1");
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseMiddleware<TokenMiddleware>();
        app.MapControllers();
        app.MapHealthChecks("/health");
        app.Run();
    }
}
