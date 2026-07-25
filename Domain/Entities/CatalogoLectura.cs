namespace Domain.Entities
{
    public class CatalogoLectura
    {
        public Guid ProductoId { get; private set; }
        public string Sku { get; private set; }
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }
        public decimal PrecioActual { get; private set; }
        public int CantidadDisponibleEstimada { get; private set; }
        public bool Activo { get; private set; }
        public DateTime FechaSincronizacion { get; private set; }

        private CatalogoLectura() { }

        public static CatalogoLectura DesdeSincronizacion(
            Guid productoId, string sku, string nombre,
            string descripcion, decimal precioActual,
            int cantidadDisponible, bool activo)
        {
            if (productoId == Guid.Empty)
                throw new ArgumentException("El productoId es obligatorio.");

            return new CatalogoLectura
            {
                ProductoId = productoId,
                Sku = sku,
                Nombre = nombre,
                Descripcion = descripcion,
                PrecioActual = precioActual,
                CantidadDisponibleEstimada = cantidadDisponible,
                Activo = activo,
                FechaSincronizacion = DateTime.UtcNow
            };
        }

        public void ActualizarDesdeSincronizacion(
            string nombre, string descripcion, decimal precioActual,
            int cantidadDisponible, bool activo)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            PrecioActual = precioActual;
            CantidadDisponibleEstimada = cantidadDisponible;
            Activo = activo;
            FechaSincronizacion = DateTime.UtcNow;
        }

        public bool TieneDisponibilidadEstimada(int cantidadSolicitada) =>
            Activo && CantidadDisponibleEstimada >= cantidadSolicitada;
    }
}
