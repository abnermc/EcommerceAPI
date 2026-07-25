using Domain.Exceptions;

namespace Domain.Entities
{
    public enum EstadoPedido
    {
        Pendiente = 1,   // carrito abierto
        Confirmado = 2,  // stock descontado, venta efectiva
        Cancelado = 3,
        Rechazado = 4    // no había stock al confirmar
    }
    public class Pedido
    {
        private readonly List<DetallePedido> _detalles = new();

        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public EstadoPedido Estado { get; private set; }
        public decimal Total { get; private set; }
        public string? DireccionEntrega { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public DateTime? FechaConfirmacion { get; private set; }
        public IReadOnlyCollection<DetallePedido> Detalles => _detalles.AsReadOnly();

        private Pedido() { }

        public Pedido(Guid clienteId, string? direccionEntrega = null)
        {
            if (clienteId == Guid.Empty)
                throw new ArgumentException("El cliente es obligatorio.");

            Id = Guid.NewGuid();
            ClienteId = clienteId;
            Estado = EstadoPedido.Pendiente;
            Total = 0;
            DireccionEntrega = direccionEntrega;
            FechaCreacion = DateTime.UtcNow;
        }

        public void AgregarDetalle(
            Guid productoId, string nombreSnapshot,
            int cantidad, decimal precioUnitarioCongelado)
        {
            if (Estado != EstadoPedido.Pendiente)
                throw new PedidoException(
                    "Solo se pueden agregar productos a un pedido pendiente.");
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.");
            if (precioUnitarioCongelado <= 0)
                throw new ArgumentException("El precio debe ser mayor a cero.");

            // Si el producto ya está en el pedido, aumenta la cantidad
            var detalleExistente = _detalles
                .FirstOrDefault(d => d.ProductoId == productoId);

            if (detalleExistente is not null)
            {
                detalleExistente.AumentarCantidad(cantidad);
            }
            else
            {
                var detalle = new DetallePedido(
                    Id, productoId, nombreSnapshot,
                    cantidad, precioUnitarioCongelado);
                _detalles.Add(detalle);
            }

            RecalcularTotal();
        }

        public void Confirmar()
        {
            if (Estado != EstadoPedido.Pendiente)
                throw new PedidoException("El pedido ya fue procesado.");
            if (_detalles.Count == 0)
                throw new PedidoException(
                    "No se puede confirmar un pedido sin productos.");

            Estado = EstadoPedido.Confirmado;
            FechaConfirmacion = DateTime.UtcNow;
        }

        public void Rechazar()
        {
            if (Estado != EstadoPedido.Pendiente)
                throw new PedidoException("El pedido ya fue procesado.");
            Estado = EstadoPedido.Rechazado;
        }

        public void Cancelar()
        {
            if (Estado == EstadoPedido.Confirmado)
                throw new PedidoException(
                    "Un pedido confirmado no se puede cancelar directamente.");
            if (Estado != EstadoPedido.Pendiente)
                throw new PedidoException("El pedido ya fue procesado.");
            Estado = EstadoPedido.Cancelado;
        }

        private void RecalcularTotal() =>
            Total = _detalles.Sum(d => d.Subtotal);
    }
}
