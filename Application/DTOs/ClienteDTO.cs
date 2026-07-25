namespace Application.DTOs
{
    public record ClienteDto
    {
        public Guid Id { get; init; }
        public string Nombres { get; init; } = string.Empty;
        public string Apellidos { get; init; } = string.Empty;
        public string NombreCompleto { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Telefono { get; init; } = string.Empty;
        public DateTime FechaRegistro { get; init; }
    }

    public record RegistrarClienteRequest
    {
        public string Nombres { get; init; } = string.Empty;
        public string Apellidos { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string Telefono { get; init; } = string.Empty;
    }

    public record LoginClienteRequest
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }

    public record LoginClienteResponse
    {
        public string Token { get; init; } = string.Empty;
        public string NombreCompleto { get; init; } = string.Empty;
        public DateTime Expiracion { get; init; }
    }
}
