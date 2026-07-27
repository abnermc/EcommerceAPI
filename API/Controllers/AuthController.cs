using Application.DTOs;
using Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RegistrarClienteUseCase _registrarClienteUseCase;
        private readonly LoginClienteUseCase _loginClienteUseCase;

        public AuthController(
            RegistrarClienteUseCase registrarClienteUseCase,
            LoginClienteUseCase loginClienteUseCase)
        {
            _registrarClienteUseCase = registrarClienteUseCase;
            _loginClienteUseCase= loginClienteUseCase;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro(RegistrarClienteRequest request)
        {
            var resultado = await _registrarClienteUseCase.EjecutarAsync(request);
            return CreatedAtAction(nameof(Registro), resultado);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginClienteRequest request)
        {
            var resultado = await _loginClienteUseCase.EjecutarAsync(request);
            return Ok(resultado);
        }
    }
}
