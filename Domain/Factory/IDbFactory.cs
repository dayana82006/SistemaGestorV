namespace SistemaGestorV.Domain.Ports
{
    public interface IDbFactory
    {
        ITerceroRepository CrearTerceroRepository();
    }
}