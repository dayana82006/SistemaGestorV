using System.Collections.Generic;

namespace SistemaGestorV.Domain.Ports;

public interface IGenericRepository<T>
{
    IEnumerable<T> ObtenerTodos();
    T ObtenerPorId(int id);
    void Crear(T entity);
    void Actualizar(T entity);
    void Eliminar(int id);
}