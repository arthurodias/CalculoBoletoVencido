namespace Boleto.Domain.Entities
{
    public class BoletoModel
    {
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string PaymentDate { get; set; }
        public string Type { get; set; }
    }
}