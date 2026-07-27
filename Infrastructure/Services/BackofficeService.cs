using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class BackofficeService : IBackofficeService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public BackofficeService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool> DescontarStockAsync(Guid productoId, int cantidad, Guid pedidoId)
        {
            try
            {
                var payload = new
                {
                    productoId = productoId,
                    cantidad = cantidad,
                    pedidoIdReferencia = pedidoId
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(
                    json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/stock/descontar", content);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<decimal?> ObtenerPrecioActualAsync(Guid productoId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/productos/{productoId}");

                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);

                return doc.RootElement.GetProperty("precioBase").GetDecimal();
            }
            catch
            {
                return null;
            }
        }
    }
}
