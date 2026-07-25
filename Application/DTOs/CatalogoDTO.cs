namespace Application.DTOs
{
    public record ProductoCatalogoDto
    {
        public Guid ProductoId { get; init; }
        public string Sku { get; init; } = string.Empty;
        public string Nombre { get; init; } = string.Empty;
        public string Descripcion { get; init; } = string.Empty;
        public decimal PrecioActual { get; init; }
        public int CantidadDisponible { get; init; }
        public bool Disponible { get; init; }
        public DateTime FechaSincronizacion { get; init; }
    }

    // Este DTO lo recibe Ecommerce cuando Backoffice hace el push
    public record SincronizarProductoRequest
    {
        public Guid ProductoId { get; init; }
        public string Sku { get; init; } = string.Empty;
        public string Nombre { get; init; } = string.Empty;
        public string Descripcion { get; init; } = string.Empty;
        public decimal PrecioActual { get; init; }
        public int CantidadDisponible { get; init; }
        public bool Activo { get; init; }
    }
}
