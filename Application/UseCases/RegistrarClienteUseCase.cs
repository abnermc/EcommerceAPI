using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
    public class RegistrarClienteUseCase
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarClienteUseCase(
            IClienteRepository clienteRepository, 
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork)
        {
            _clienteRepository = clienteRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClienteDto> EjecutarAsync(RegistrarClienteRequest request)
        {
            var existente = await _clienteRepository.ObtenerPorEmailAsync(request.Email);
            if (existente is not null) throw new InvalidOperationException("Ya existe una cuenta con ese Email.");

            var hash = _passwordHasher.Hash(request.Password);

            var cliente = new Cliente(
                request.Nombres,
                request.Apellidos,
                request.Email,
                hash,
                request.Telefono
            );

            await _clienteRepository.AgregarAsync(cliente);
            await _unitOfWork.CommitAsync();

            return new ClienteDto
            {
                Id = cliente.Id,
                Nombres = cliente.Nombres,
                Apellidos = cliente.Apellidos,
                NombreCompleto = cliente.NombreCompleto,
                Email = cliente.Email,
                Telefono = cliente.Telefono,
                FechaRegistro = cliente.FechaRegistro
            };
        }
    }
}
