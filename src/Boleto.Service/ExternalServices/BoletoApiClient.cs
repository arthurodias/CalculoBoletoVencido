using System.Net.Http.Headers;
using System.Text;
using Boleto.Domain.Entities;
using Boleto.Domain.Intefaces.Proxy;
using Boleto.Domain.Models;
using Newtonsoft.Json;

namespace Boleto.Service.ExternalServices
{
    public class BoletoApiClient : IBoletoApiClient
    {
        private readonly HttpClient _httpClient;

        public BoletoApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BoletoModel> GetBoletoAsync(string code)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://vagas.builders/api/builders/bill-payments/codes");
            request.Headers.Authorization = new AuthenticationHeaderValue(await GetTokenAsync());

            var payload = new { code = code };
            request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var boletoData = JsonConvert.DeserializeObject<BoletoApiResponse>(content);

            return new BoletoModel
            {
                Code = boletoData.Code,
                DueDate = DateTime.Parse(boletoData.DueDate),
                Amount = boletoData.Amount,
                Type = boletoData.Type
            };
        }

        private async Task<string> GetTokenAsync()
        {
            var tokenRequest = new
            {
                client_id = "bd753592-cf9b-4d1a-96b9-cb8b2c01bd12",
                client_secret = "4e8229fe-1131-439c-9846-799895a8be5b"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://vagas.builders/api/builders/auth/tokens");
            request.Content = new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);

            return tokenResponse.Token;
        }
    }
}