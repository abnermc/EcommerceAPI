using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICatalogoLecturaRepository
    {
        Task<CatalogoLectura?> ObtenerPorProductoIdAsync(Guid productoId);
        Task<IEnumerable<CatalogoLectura>> ObtenerTodosActivosAsync();
        Task<IEnumerable<CatalogoLectura>> BuscarAsync(string termino);
        Task AgregarAsync(CatalogoLectura catalogo);
        Task ActualizarAsync(CatalogoLectura catalogo);
    }
}
