namespace Domain.Entities
{
    public class DetallePedido
    {
        public Guid Id { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid ProductoId { get; private set; }
        public string NombreProductoSnapshot { get; private set; }
        public int Cantidad { get; private set; }
        public decimal PrecioUnitarioCongelado { get; private set; }
        public decimal Subtotal { get; private set; }

        private DetallePedido() { }

        internal DetallePedido(
            Guid pedidoId, Guid productoId, string nombreSnapshot,
            int cantidad, decimal precioUnitarioCongelado)
        {
            Id = Guid.NewGuid();
            PedidoId = pedidoId;
            ProductoId = productoId;
            NombreProductoSnapshot = nombreSnapshot;
            Cantidad = cantidad;
            PrecioUnitarioCongelado = precioUnitarioCongelado;
            Subtotal = cantidad * precioUnitarioCongelado;
        }

        internal void AumentarCantidad(int cantidad)
        {
            Cantidad += cantidad;
            Subtotal = Cantidad * PrecioUnitarioCongelado;
        }
    }
}
