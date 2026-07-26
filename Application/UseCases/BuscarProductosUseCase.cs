using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases
{
    public class BuscarProductosUseCase
    {
        public readonly ICatalogoLecturaRepository _catalogoRepository;
        public BuscarProductosUseCase(ICatalogoLecturaRepository catalogoRepository)
        {
            _catalogoRepository = catalogoRepository;
        }

        public async Task<IEnumerable<ProductoCatalogoDto>> EjecutarAsync(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino)) throw new ArgumentException("Ingrese un término de búsqueda.");

            var productos = await _catalogoRepository.BuscarAsync(termino);

            return productos.Select(p => new ProductoCatalogoDto
            {
                ProductoId = p.ProductoId,
                Sku = p.Sku,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                PrecioActual = p.PrecioActual,
                CantidadDisponible = p.CantidadDisponibleEstimada,
                Disponible = p.TieneDisponibilidadEstimada(1),
                FechaSincronizacion = p.FechaSincronizacion
            });
        }
    }
}
