namespace SistemaGestorV.Domain.Entities
{
    public class DetalleCompra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string ProductoId { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public double Valor { get; set; }
        public int CompraId { get; set; }
    }
}
