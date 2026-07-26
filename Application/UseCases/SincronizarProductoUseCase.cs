using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
    public class SincronizarProductoUseCase
    {
        private readonly ICatalogoLecturaRepository _catalogoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SincronizarProductoUseCase(
            ICatalogoLecturaRepository catalogoRepository,
            IUnitOfWork unitOfWork)
        {
            _catalogoRepository = catalogoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task EjecutarAsync(SincronizarProductoRequest request)
        {
            var existente = await _catalogoRepository.ObtenerPorProductoIdAsync(request.ProductoId);

            if (existente is null)
            {
                // Primera vez que Backoffice notifica el producto
                var nuevo = CatalogoLectura.DesdeSincronizacion(
                    request.ProductoId,
                    request.Sku,
                    request.Nombre,
                    request.Descripcion,
                    request.PrecioActual,
                    request.CantidadDisponible,
                    request.Activo
                );
                await _catalogoRepository.AgregarAsync(nuevo);
            }
            else
            {
                // Actualiza copia existente
                existente.ActualizarDesdeSincronizacion(
                    request.Nombre,
                    request.Descripcion,
                    request.PrecioActual,
                    request.CantidadDisponible,
                    request.Activo
                );
                await _catalogoRepository.ActualizarAsync(existente);
            }
            await _unitOfWork.CommitAsync();

        }
    }
}
