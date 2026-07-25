using Domain.Entities;

namespace Application.Interfaces
{
    public record TokenJwt(string Valor, DateTime Expiracion);
    public interface IJwtService
    {
        TokenJwt GenerarToken(Cliente cliente);
    }
}
