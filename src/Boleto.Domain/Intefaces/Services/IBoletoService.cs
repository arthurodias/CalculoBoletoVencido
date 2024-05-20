using Boleto.Domain.Models;

namespace Boleto.Domain.Intefaces.Services
{
    public interface IBoletoService
    {
        Task<BoletoResponse> CalcularValorBoletoAsync(string barCode, DateTime paymentDate, string token);
    }
}