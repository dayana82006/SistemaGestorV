
namespace SistemaGestorV.Domain.Entities
{
    public class Compra
    {
        public int Id { get; set; }
        public int TerceroProvId { get; set; }
        public DateTime Fecha { get; set; }
        public int TerceroEmpId { get; set; }
        public string DocCompra { get; set; } = string.Empty;
        public List<DetalleCompra> Detalles { get; internal set; }

        public Compra()
        {
            Detalles = new List<DetalleCompra>();
            Fecha = DateTime.Now.Date; 
        }
        public Compra(int terceroProvId, DateTime fecha, int terceroEmpId, string docCompra)
        {
            TerceroProvId = terceroProvId;
            Fecha = fecha.Date; 
            TerceroEmpId = terceroEmpId;
            DocCompra = docCompra;
            Detalles = new List<DetalleCompra>();
        }

    }
}
