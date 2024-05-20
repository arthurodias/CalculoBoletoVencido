using Newtonsoft.Json;

namespace Boleto.Api.Models
{
    public class BoletoRequest
    {
        [JsonProperty("bar_code")]
        public string BarCode { get; set; }
        [JsonProperty("payment_date")]
        public string PaymentDate { get; set; }
    }
}