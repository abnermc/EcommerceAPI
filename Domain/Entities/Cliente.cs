namespace Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; private set; }
        public string Nombres { get; private set; }
        public string Apellidos { get; private set; }
        public string NombreCompleto => $"{Nombres} {Apellidos}";
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string Telefono { get; private set; }
        public bool Activo { get; private set; }
        public DateTime FechaRegistro { get; private set; }

        private Cliente() { }

        public Cliente(string nombres, string apellidos,
                       string email, string passwordHash, string telefono)
        {
            if (string.IsNullOrWhiteSpace(nombres))
                throw new ArgumentException("Los nombres son obligatorios.");
            if (string.IsNullOrWhiteSpace(apellidos))
                throw new ArgumentException("Los apellidos son obligatorios.");
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                throw new ArgumentException("Email inválido.");
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("La contraseña es obligatoria.");

            Id = Guid.NewGuid();
            Nombres = nombres.Trim();
            Apellidos = apellidos.Trim();
            Email = email.ToLowerInvariant().Trim();
            PasswordHash = passwordHash;
            Telefono = telefono?.Trim() ?? string.Empty;
            Activo = true;
            FechaRegistro = DateTime.UtcNow;
        }

        public void Desactivar() => Activo = false;
    }
}
