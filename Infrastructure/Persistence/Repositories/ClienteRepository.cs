using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly EcommerceDbContext _context;
        public ClienteRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente?> ObtenerPorIdAsync(Guid id)
            => await _context.Clientes.FindAsync(id);

        public async Task<Cliente?> ObtenerPorEmailAsync(string email)
            => await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email.ToLowerInvariant());

        public async Task AgregarAsync(Cliente cliente)
            => await _context.Clientes.AddAsync(cliente);

        public async Task ActualizarAsync(Cliente cliente)
            => _context.Clientes.Update(cliente);
    }
}
