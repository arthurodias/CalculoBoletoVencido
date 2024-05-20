using Boleto.Domain.Intefaces.Proxy;
using Boleto.Domain.Intefaces.Repositories;
using Boleto.Domain.Intefaces.Services;
using Boleto.Domain.Models;

namespace Boleto.Service.Services
{
    public class BoletoService : IBoletoService
    {
        private readonly IBoletoRepository _boletoRepository;
        private readonly IBoletoApiClient _apiClient;

        public BoletoService(IBoletoRepository boletoRepository, IBoletoApiClient apiClient)
        {
            _boletoRepository = boletoRepository;
            _apiClient = apiClient;
        }

        public async Task<BoletoResponse> CalcularValorBoletoAsync(string barCode, DateTime paymentDate, string token)
        {
            var boleto = await _apiClient.GetBoletoAsync(barCode, token);

            if (boleto.Type != "NPC")
                throw new ArgumentException("Apenas boletos do tipo NPC podem ser calculados.");

            if (boleto.DueDate >= paymentDate)
                throw new ArgumentException("O boleto não está vencido.");

            int daysLate = (paymentDate - boleto.DueDate).Days;
            decimal interest = daysLate * 0.033m * boleto.Amount;
            decimal fine = 0.02m * boleto.Amount;
            decimal amount = boleto.Amount + interest + fine;

            var result = new BoletoResponse
            {
                OriginalAmount = boleto.Amount,
                Amount = amount,
                DueDate = boleto.DueDate.ToString("yyyy-MM-dd"),
                PaymentDate = paymentDate.ToString("yyyy-MM-dd"),
                InterestAmountCalculated = interest,
                FineAmountCalculated = fine
            };

            //await _boletoRepository.Save(boleto);

            return result;
        }
    
    }
}