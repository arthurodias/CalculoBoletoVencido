namespace Boleto.Domain.Entities
{
    public class BoletoEntity
    {
        public Guid Id { get; set; }
        public string BarCode { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal InterestAmountCalculated { get; set; }
        public decimal FineAmountCalculated { get; set; }
    }
}