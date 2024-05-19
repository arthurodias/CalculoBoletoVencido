using Boleto.Api.Middleware;
using Boleto.Domain.Intefaces.Repositories;
using Boleto.Domain.Intefaces.Services;
using Boleto.Infrastructure.Repositories;
using Boleto.Service.ExternalServices;
using Boleto.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBoletoService, BoletoService>();
builder.Services.AddScoped<IBoletoRepository, BoletoRepository>();

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
app.UseMiddleware<TokenMiddleware>();
app.MapControllers();
app.Run();