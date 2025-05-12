using SistemaGestorV.Domain.Entities;
using System.Collections.Generic;

public interface IRegionRepository
{
    void Crear(Region region);
    void Actualizar(Region region);
    void Eliminar(string id); 
    Region ObtenerPorId(string id); 
    IEnumerable<Region> ObtenerTodos();
}
