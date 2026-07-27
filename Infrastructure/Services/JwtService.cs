using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenJwt GenerarToken(Cliente cliente)
        {
            var clave = _configuration["Jwt:Clave"]
                ?? throw new InvalidOperationException("Jwt: Clave no configurada.");

            var expiracionHoras = int.Parse(_configuration["Jwt:ExpiracionHoras"] ?? "24");

            var expiracion = DateTime.UtcNow.AddHours(expiracionHoras);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
                new Claim(ClaimTypes.Email, cliente.Email),
                new Claim(ClaimTypes.GivenName, cliente.Nombres),
                new Claim(ClaimTypes.Surname, cliente.Apellidos),
                new Claim(ClaimTypes.Role, "Cliente")
            };

            var claveBytes = Encoding.UTF8.GetBytes(clave);
            var credenciales = new SigningCredentials(
                new SymmetricSecurityKey(claveBytes),
                SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Emisor"],
                audience: _configuration["Jwt:Audiencia"],
                claims: claims,
                expires: expiracion,
                signingCredentials: credenciales
            );

            return new TokenJwt(
                new JwtSecurityTokenHandler().WriteToken(token),
                expiracion
                );
        }
    }
}
