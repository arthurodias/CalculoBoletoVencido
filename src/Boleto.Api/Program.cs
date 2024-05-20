using Boleto.Domain.Intefaces.Proxy;
using Boleto.Domain.Intefaces.Repositories;
using Boleto.Domain.Intefaces.Services;
using Boleto.Infrastructure.Repositories;
using Boleto.Service.ExternalServices;
using Boleto.Service.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
        builder.Services.AddSwaggerGen();
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
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        //app.UseMiddleware<TokenMiddleware>();
        app.MapControllers();
        // Configura o endpoint do HealthCheck
        app.MapHealthChecks("/health");
        app.Run();
    }
}
