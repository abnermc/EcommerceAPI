using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
    public class CrearPedidoUseCase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CrearPedidoUseCase(
            IPedidoRepository pedidoRepository,
            IUnitOfWork unitOfWork)
        {
            _pedidoRepository = pedidoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PedidoDto> EjecutarAsync(Guid clienteId, CrearPedidoRequest request)
        {
            var pedido = new Pedido(clienteId, request.DireccionEntrega);

            await _pedidoRepository.AgregarAsync(pedido);
            await _unitOfWork.CommitAsync();

            return new PedidoDto
            {
                Id = pedido.Id,
                ClienteId = pedido.ClienteId,
                Estado = pedido.Estado.ToString(),
                Total = pedido.Total,
                DireccionEntrega = pedido.DireccionEntrega,
                FechaCreacion = pedido.FechaCreacion,
                Detalles = Enumerable.Empty<DetallePedidoDto>()
            };
        }
    }
}
