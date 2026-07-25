namespace Application.Interfaces
{
    public interface IBackofficeService
    {
        // Valida y descuenta stock en backoffice al confirmar pedido
        Task<bool> DescontarStock(Guid productoId, int cantidad, Guid pedidoId);

        // Consulta el precio actual de un producto en backoffice para congelarlo al momento de confirmar
        Task<decimal?> ObtenerPrecioActualAsync(Guid productoId);
    }
}
