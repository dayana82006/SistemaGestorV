namespace SistemaGestorV.Domain.Entities;

public class Telefono
{
    public int Id { get; set; }
    public string Numero { get; set; }
    public string Tipo { get; set; }
    public int TerceroId { get; set; }
}