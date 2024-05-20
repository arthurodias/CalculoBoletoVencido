using Newtonsoft.Json;

namespace Boleto.Domain.Models
{
    public class TokenResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}