using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly EcommerceDbContext _context;

        public PedidoRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido?> ObtenerPorIdAsync(Guid id)
            => await _context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Pedido>> ObtenerPorClienteIdAsync(Guid clienteId)
            => await _context.Pedidos
                .Include(p => p.Detalles)
                .Where(p => p.ClienteId == clienteId)
                .OrderByDescending(p => p.FechaConfirmacion)
                .ToListAsync();

        public async Task AgregarAsync(Pedido pedido)
            => await _context.Pedidos.AddAsync(pedido);

        public async Task ActualizarAsync(Pedido pedido)
            => _context.Pedidos.Update(pedido);
    }
}
