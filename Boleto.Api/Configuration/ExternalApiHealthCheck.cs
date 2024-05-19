using Microsoft.Extensions.Diagnostics.HealthChecks;

public class ExternalApiHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;

    public ExternalApiHealthCheck(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("https://vagas.builders/api/builders/auth/tokens");
        
        if (response.IsSuccessStatusCode)
        {
            return HealthCheckResult.Healthy("A API externa está disponível.");
        }

        return HealthCheckResult.Unhealthy("A API externa não está disponível.");
    }
}
