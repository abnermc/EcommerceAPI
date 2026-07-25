namespace Application.DTOs
{
    public record PedidoDto
    {
        public Guid Id { get; init; }
        public Guid ClienteId { get; init; }
        public string Estado { get; init; } = string.Empty;
        public decimal Total { get; init; }
        public string? DireccionEntrega { get; init; }
        public DateTime FechaCreacion { get; init; }
        public DateTime? FechaConfirmacion { get; init; }
        public IEnumerable<DetallePedidoDto> Detalles { get; init; }
            = Enumerable.Empty<DetallePedidoDto>();
    }

    public record DetallePedidoDto
    {
        public Guid ProductoId { get; init; }
        public string NombreProducto { get; init; } = string.Empty;
        public int Cantidad { get; init; }
        public decimal PrecioUnitario { get; init; }
        public decimal Subtotal { get; init; }
    }

    public record CrearPedidoRequest
    {
        public string? DireccionEntrega { get; init; }
    }

    public record AgregarDetallePedidoRequest
    {
        public Guid ProductoId { get; init; }
        public int Cantidad { get; init; }
    }
}
