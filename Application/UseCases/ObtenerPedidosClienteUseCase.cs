using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases
{
    public class ObtenerPedidosClienteUseCase
    {
        private readonly IPedidoRepository _pedidoRepository;

        public ObtenerPedidosClienteUseCase(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }
        
        public async Task<IEnumerable<PedidoDto>> EjecutarAsync(Guid clienteId)
        {
            var pedidos = await _pedidoRepository.ObtenerPorClienteIdAsync(clienteId);

            return pedidos.Select(pedido => new PedidoDto
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
            });
        }
    }
}
