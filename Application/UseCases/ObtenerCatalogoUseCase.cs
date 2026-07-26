using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases
{
    public class ObtenerCatalogoUseCase
    {
        private readonly ICatalogoLecturaRepository _catalogoLecturaRepository;

        public ObtenerCatalogoUseCase(ICatalogoLecturaRepository catalogoLecturaRepository)
        {
            _catalogoLecturaRepository = catalogoLecturaRepository;
        }

        public async Task<IEnumerable<ProductoCatalogoDto>> EjecutarAsync()
        {
            var productos = await _catalogoLecturaRepository.ObtenerTodosActivosAsync();

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
