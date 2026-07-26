using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class CatalogoLecturaRepository : ICatalogoLecturaRepository
    {
        private readonly EcommerceDbContext _context;

        public CatalogoLecturaRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<CatalogoLectura?> ObtenerPorProductoIdAsync(Guid productoId)
            => await _context.CatalogoLectura.FindAsync(productoId);

        public async Task<IEnumerable<CatalogoLectura>> ObtenerTodosActivosAsync()
            => await _context.CatalogoLectura
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

        public async Task<IEnumerable<CatalogoLectura>> BuscarAsync(string termino)
            => await _context.CatalogoLectura
                .Where(c => c.Activo && (c.Nombre.Contains(termino) || c.Sku.Contains(termino) || c.Descripcion.Contains(termino)))
                .OrderBy(c => c.Nombre)
                .ToListAsync();

        public async Task AgregarAsync(CatalogoLectura catalogo)
            => await _context.CatalogoLectura.AddAsync(catalogo);

        public async Task ActualizarAsync(CatalogoLectura catalogo)
            => _context.CatalogoLectura.Update(catalogo);
    }
}
