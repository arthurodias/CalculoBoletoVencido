using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;

namespace Boleto.Api.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public TokenMiddleware(RequestDelegate next, IMemoryCache cache, IConfiguration configuration)
        {
            _next = next;
            _cache = cache;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!_cache.TryGetValue("AuthToken", out string token))
            {
                token = await GetTokenAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1)); // Expires in 1 hour

                _cache.Set("AuthToken", token, cacheEntryOptions);
            }

            context.Request.Headers["Authorization"] = $"Bearer {token}";

            await _next(context);
        }

        private async Task<string> GetTokenAsync()
        {
            using var client = new HttpClient();
            var request = new
            {
                client_id = _configuration["BoletoApi:ClientId"],
                client_secret = _configuration["BoletoApi:ClientSecret"]
            };

            var response = await client.PostAsync("https://vagas.builders/api/builders/auth/tokens",
                new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic tokenResponse = JsonSerializer.Deserialize<dynamic>(responseContent);

            return tokenResponse.token;
        }
    }
}