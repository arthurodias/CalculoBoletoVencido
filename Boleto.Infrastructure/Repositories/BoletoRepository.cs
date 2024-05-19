using Boleto.Domain.Entities;
using Boleto.Domain.Intefaces.Repositories;

namespace Boleto.Infrastructure.Repositories
{
    public class BoletoRepository : IBoletoRepository
    {
        public BoletoRepository()
        {

        }

        public Task Save(BoletoModel boleto)
        {
            throw new NotImplementedException();
        }
    }
}