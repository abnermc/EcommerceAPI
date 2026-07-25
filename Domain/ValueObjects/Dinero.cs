namespace Domain.ValueObjects
{
    public record Dinero
    {
        public decimal Monto { get; }

        public Dinero(decimal monto)
        {
            if (monto < 0) throw new ArgumentException("El monto no puede ser negativo.");
            Monto = Math.Round(monto, 2);
        }

        public static Dinero Cero => new(0);

        public Dinero Sumar(Dinero otro) => new(Monto + otro.Monto);
        public Dinero Multiplicar(int cantidad) => new(Monto * cantidad);

        public static implicit operator decimal(Dinero dinero) => dinero.Monto;

        public override string ToString() => $"S/ {Monto:F2}";
    }
}
