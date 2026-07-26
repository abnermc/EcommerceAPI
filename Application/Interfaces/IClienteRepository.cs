using Domain.Entities;

namespace Application.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente?> ObtenerPorIdAsync(Guid id);
        Task<Cliente?> ObtenerPorEmailAsync(string email);
        Task AgregarAsync(Cliente cliente);
        Task ActualizarAsync(Cliente cliente);
    }
}
