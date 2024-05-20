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

        public async Task<BoletoModel> GetBoletoAsync(string code, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://vagas.builders/api/builders/bill-payments/codes");
            request.Headers.Authorization = new AuthenticationHeaderValue(token);

            var payload = new { code };
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
    }
}