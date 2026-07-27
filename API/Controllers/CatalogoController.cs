using Application.DTOs;
using Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogoController : ControllerBase
    {
        private readonly ObtenerCatalogoUseCase _obtenerCatalogoUseCase;
        private readonly BuscarProductosUseCase _buscarProductosUseCase;
        private readonly SincronizarProductoUseCase _sincronizarProductoUseCase;

        public CatalogoController(
            ObtenerCatalogoUseCase obtenerCatalogoUseCase,
            BuscarProductosUseCase buscarProductosUseCase,
            SincronizarProductoUseCase sincronizarProductoUseCase)
        {
            _obtenerCatalogoUseCase = obtenerCatalogoUseCase;
            _buscarProductosUseCase = buscarProductosUseCase;
            _sincronizarProductoUseCase = sincronizarProductoUseCase;
        }

        // Público, cualquier visitante puede ver el catálogo sin login
        [HttpGet]
        public async Task<IActionResult> ObtenerCatalogo()
        {
            var resultado = await _obtenerCatalogoUseCase.EjecutarAsync();
            return Ok(resultado);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string termino)
        {
            var resultado = await _buscarProductosUseCase.EjecutarAsync(termino);
            return Ok(resultado);
        }

        // Este endpoint será llamado por el backoffice, no el cliente
        [HttpPost("sincronizar")]
        public async Task<IActionResult> Sincronizar(SincronizarProductoRequest request)
        {
            await _sincronizarProductoUseCase.EjecutarAsync(request);
            return Ok();
        }
    }
}
