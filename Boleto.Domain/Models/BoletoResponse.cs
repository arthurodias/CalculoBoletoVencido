using Newtonsoft.Json;

namespace Boleto.Domain.Models
{
    public class BoletoResponse
    {
        [JsonProperty("original_amount")]
        public decimal OriginalAmount { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("due_date")]
        public string DueDate { get; set; }
        [JsonProperty("payment_date")]
        public string PaymentDate { get; set; }
        [JsonProperty("interest_amount_calculated")]
        public decimal InterestAmountCalculated { get; set; }
        [JsonProperty("fine_amount_calculated")]
        public decimal FineAmountCalculated { get; set; }
    }
}