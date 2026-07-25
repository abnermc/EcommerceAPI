using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido?> ObtenerPorIdAsync(Guid id);
        Task<IEnumerable<Pedido>> ObtenerPorClienteIdAsync(Guid clienteId);
        Task AgregarAsync(Pedido pedido);
        Task ActualizarAsync(Pedido pedido);
    }
}
