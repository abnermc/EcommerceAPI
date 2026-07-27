using Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await ManejarExcepcionAsync(context, ex);
            }
        }

        private async Task ManejarExcepcionAsync(HttpContext context, Exception ex)
        {
            var (statusCode, mensaje) = ex switch
            {
                StockInsuficienteException e =>
                    (HttpStatusCode.Conflict,
                    $"Stock insuficiente. Disponible: {e.CantidadDisponible}, " +
                    $"Solicitado: {e.CantidadSolicitada}"),

                PedidoException e =>
                    (HttpStatusCode.BadRequest, e.Message),

                InvalidOperationException e =>
                    (HttpStatusCode.BadRequest, e.Message),

                UnauthorizedAccessException =>
                    (HttpStatusCode.Unauthorized, "No autorizado."),

                ArgumentException e =>
                    (HttpStatusCode.BadRequest, e.Message),

                _ =>
                    (HttpStatusCode.InternalServerError,
                     "Ocurrió un error interno. Intenta de nuevo.")
            };

            if (statusCode == HttpStatusCode.InternalServerError)
                _logger.LogError(ex, "Error no controlado.");
            else
                _logger.LogWarning(ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var respuesta = new
            {
                error = mensaje,
                statusCode = (int)statusCode
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(respuesta));
        }
    }
}
