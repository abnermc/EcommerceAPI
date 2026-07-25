namespace Domain.Exceptions
{
    public class PedidoException : Exception
    {
        public PedidoException(string mensaje) : base(mensaje) { }
    }

    public class StockInsuficienteException : Exception
    {
        public Guid ProductoId { get; }
        public int CantidadDisponible { get; }
        public int CantidadSolicitada { get; }
        public StockInsuficienteException(
            Guid productoId,
            int cantidadDisponible,
            int cantidadSolicitada)
            :base($"Stock insuficiente para el producto {productoId}. " + $"Disponible: {cantidadDisponible}, Solicitado: {cantidadSolicitada}")
        {
            ProductoId = productoId;
            CantidadDisponible = cantidadDisponible;
            CantidadSolicitada = cantidadSolicitada;
        }
    }
}
