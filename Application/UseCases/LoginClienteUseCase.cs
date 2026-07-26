using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases
{
    public class LoginClienteUseCase
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;

        public LoginClienteUseCase(
            IClienteRepository clienteRepository,
            IPasswordHasher passwordHasher,
            IJwtService jwtService)
        {
            _clienteRepository = clienteRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<LoginClienteResponse> EjecutarAsync(LoginClienteRequest request)
        {
            var cliente = await _clienteRepository.ObtenerPorEmailAsync(request.Email);
            if (cliente is null || !cliente.Activo)
                throw new UnauthorizedAccessException("Credenciales inválidas.");

            var passwordValida = _passwordHasher.Verificar(request.Password, cliente.PasswordHash);
            if (!passwordValida) throw new UnauthorizedAccessException("Credenciales inválidas.");

            var token = _jwtService.GenerarToken(cliente);

            return new LoginClienteResponse
            {
                Token = token.Valor,
                NombreCompleto = cliente.NombreCompleto,
                Expiracion = token.Expiracion
            };
        }
    }
}
