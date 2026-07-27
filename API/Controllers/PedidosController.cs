using Application.DTOs;
using Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/controller")]
    [Authorize] // Todos los pedidos requieren login de cliente
    public class PedidosController : ControllerBase
    {
        private readonly CrearPedidoUseCase _crearPedidoUseCase;
        private readonly AgregarDetallePedidoUseCase _agregarDetalleUseCase;
        private readonly ConfirmarPedidoUseCase _confirmarPedidoUseCase;
        private readonly ObtenerPedidosClienteUseCase _obtenerPedidosUseCase;

        public PedidosController(
            CrearPedidoUseCase crearPedidoUseCase,
            AgregarDetallePedidoUseCase agregarDetalleUseCase,
            ConfirmarPedidoUseCase confirmarPedidoUseCase,
            ObtenerPedidosClienteUseCase obtenerPedidosUseCase)
        {
            _crearPedidoUseCase = crearPedidoUseCase;
            _agregarDetalleUseCase = agregarDetalleUseCase;
            _confirmarPedidoUseCase = confirmarPedidoUseCase;
            _obtenerPedidosUseCase = obtenerPedidosUseCase;
        }

        // Crea un pedido vacío
        [HttpPost]
        public async Task<IActionResult> Crear(CrearPedidoRequest request)
        {
            var clienteId = ObtenerClienteId();
            var resultado = await _crearPedidoUseCase.EjecutarAsync(clienteId, request);

            return CreatedAtAction(nameof(ObtenerMisPedidos), new { id = resultado.Id }, resultado);
        }

        // Agregar un producto al carrito
        [HttpPost("{pedidoId}/detalles")]
        public async Task<IActionResult> AgregarDetalle(Guid pedidoId, AgregarDetallePedidoRequest request)
        {
            var resultado = await _agregarDetalleUseCase.EjecutarAsync(pedidoId, request);
            return Ok(resultado);
        }

        // Confirma el pedido - descuenta stock en backoffice
        [HttpPost("{pedidoId}/confirmar")]
        public async Task<IActionResult> Confirmar(Guid pedidoId)
        {
            var resultado = await _confirmarPedidoUseCase.EjecutarAsync(pedidoId);
            return Ok(resultado);
        }

        // Historial de pedidos de cliente autenticado
        [HttpGet("mis-pedidos")]
        public async Task<IActionResult> ObtenerMisPedidos()
        {
            var clienteId = ObtenerClienteId();
            var resultado = await _obtenerPedidosUseCase.EjecutarAsync(clienteId);

            return Ok(resultado);
        }

        private Guid ObtenerClienteId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("No se pudo identificar al cliente");
            return Guid.Parse(claim);
        }
    }
}
