using Boleto.Domain.Entities;
using Boleto.Domain.Intefaces.Repositories;
using Boleto.Infrastructure.Data;

namespace Boleto.Infrastructure.Repositories
{
    public class BoletoRepository : IBoletoRepository
    {
        private readonly BoletoDbContext _context;

        public BoletoRepository(BoletoDbContext context)
        {
            _context = context;
        }

        public async Task Salvar(BoletoEntity boleto)
        {
            _context.Boletos.Add(boleto);
            await _context.SaveChangesAsync();
        }
    }
}