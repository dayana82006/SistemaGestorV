namespace SistemaGestorV.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public int TerceroId { get; set; }
    public DateTime FechaNac { get; set; }
    public DateTime FechaInforma { get; set; }
}