using Boleto.Domain.Entities;

namespace Boleto.Domain.Intefaces.Proxy
{
    public interface IBoletoApiClient
    {
        Task<BoletoModel> GetBoletoAsync(string code, string token);
    }
}