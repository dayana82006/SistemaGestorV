namespace SistemaGestorV.Domain.Entities;

public class Empleado
{
    public int Id { get; set; }
    public int TerceroId { get; set; }
    public DateTime FechaIngreso { get; set; }
    public double SalarioBase { get; set; }
    public int EpsId { get; set; }
    public int ArlId { get; set; }
}