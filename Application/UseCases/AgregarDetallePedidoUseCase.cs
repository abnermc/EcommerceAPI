using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases
{
    public class AgregarDetallePedidoUseCase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ICatalogoLecturaRepository _catalogoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AgregarDetallePedidoUseCase(
            IPedidoRepository pedidoRepository,
            ICatalogoLecturaRepository catalogoRepository,
            IUnitOfWork unitOfWork)
        {
            _pedidoRepository = pedidoRepository;
            _catalogoRepository = catalogoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PedidoDto> EjecutarAsync(Guid pedidoId, AgregarDetallePedidoRequest request)
        {
            var pedido = await _pedidoRepository.ObtenerPorIdAsync(pedidoId)
                ?? throw new InvalidOperationException("El pedido no existe.");

            var catalogo = await _catalogoRepository.ObtenerPorProductoIdAsync(request.ProductoId)
                ?? throw new InvalidOperationException("El producto no existe en el catálogo.");

            if (!catalogo.TieneDisponibilidadEstimada(request.Cantidad))
                throw new InvalidOperationException("El producto no tiene stock disponible.");

            // El precio se congela aquí, al agregar al carrito 
            pedido.AgregarDetalle(
                request.ProductoId,
                catalogo.Nombre,
                request.Cantidad,
                catalogo.PrecioActual
            );

            await _pedidoRepository.ActualizarAsync(pedido);
            await _unitOfWork.CommitAsync();

            return new PedidoDto
            {
                Id = pedido.Id,
                ClienteId = pedido.ClienteId,
                Estado = pedido.Estado.ToString(),
                Total = pedido.Total,
                DireccionEntrega = pedido.DireccionEntrega,
                FechaCreacion = pedido.FechaCreacion,
                Detalles = pedido.Detalles.Select(d => new DetallePedidoDto
                {
                    ProductoId = d.ProductoId,
                    NombreProducto = d.NombreProductoSnapshot,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitarioCongelado,
                    Subtotal = d.Subtotal
                })
            };  
        }
    }
}
