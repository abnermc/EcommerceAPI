using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases
{
    public class ConfirmarPedidoUseCase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ICatalogoLecturaRepository _catalogoRepository;
        private readonly IBackofficeService _backofficeService;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmarPedidoUseCase(
            IPedidoRepository pedidoRepository,
            ICatalogoLecturaRepository catalogoRepository,
            IBackofficeService backofficeService,
            IUnitOfWork unitOfWork
            )
        {
            _pedidoRepository = pedidoRepository;
            _catalogoRepository = catalogoRepository;
            _backofficeService = backofficeService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PedidoDto> EjecutarAsync(Guid pedidoId)
        {
            var pedido = await _pedidoRepository.ObtenerPorIdAsync(pedidoId)
                ?? throw new InvalidOperationException("El pedido no existe.");

            // Validación rápida contra copia local
            foreach (var detalle in pedido.Detalles)
            {
                var catalogo = await _catalogoRepository.ObtenerPorProductoIdAsync(detalle.PedidoId);

                if (catalogo is null || !catalogo.TieneDisponibilidadEstimada(detalle.Cantidad)){
                    pedido.Rechazar();
                    await _pedidoRepository.ActualizarAsync(pedido);
                    await _unitOfWork.CommitAsync();
                    throw new InvalidOperationException(
                        $"El producto '{detalle.NombreProductoSnapshot}' " +
                        $"no tiene stock suficiente.");
                }
            }

            // Descuento real en backoffice
            foreach (var detalle in pedido.Detalles)
            {
                var exito = await _backofficeService.DescontarStockAsync(
                    detalle.ProductoId,
                    detalle.Cantidad,
                    pedido.Id
                );

                if (!exito)
                {
                    pedido.Rechazar();
                    await _pedidoRepository.ActualizarAsync(pedido);
                    await _unitOfWork.CommitAsync();
                    throw new InvalidOperationException(
                        $"No se pudo descontar el stock del producto " +
                        $"'{detalle.NombreProductoSnapshot}'. " +
                        $"Puede que se haya agotado en este momento.");
                }
            }

            // Solo si backoffice confirmó todo, el pedido se confirma
            pedido.Confirmar();
            await _pedidoRepository.ActualizarAsync(pedido);
            await _unitOfWork.CommitAsync();

            return MapearDto(pedido);
        }

         private static PedidoDto MapearDto(Domain.Entities.Pedido pedido) =>
             new PedidoDto
             {
                 Id = pedido.Id,
                 ClienteId = pedido.ClienteId,
                 Estado = pedido.Estado.ToString(),
                 Total = pedido.Total,
                 DireccionEntrega = pedido.DireccionEntrega,
                 FechaCreacion = pedido.FechaCreacion,
                 FechaConfirmacion = pedido.FechaConfirmacion,
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
