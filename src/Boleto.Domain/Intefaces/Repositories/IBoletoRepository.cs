using Boleto.Domain.Entities;

namespace Boleto.Domain.Intefaces.Repositories
{
    public interface IBoletoRepository
    {
        Task Save(BoletoModel boleto);
    }
}