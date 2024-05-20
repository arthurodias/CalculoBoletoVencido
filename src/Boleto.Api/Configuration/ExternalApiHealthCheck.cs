using System.Text;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;


public class ExternalApiHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;


    public ExternalApiHealthCheck(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var request = new
        {
            client_id = _configuration["BoletoApi:ClientId"],
            client_secret = _configuration["BoletoApi:ClientSecret"]
        };

        var response = await _httpClient.PostAsync("https://vagas.builders/api/builders/auth/tokens", new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            return HealthCheckResult.Healthy("A API externa está disponível.");
        }

        return HealthCheckResult.Unhealthy("A API externa não está disponível.");
    }
}
