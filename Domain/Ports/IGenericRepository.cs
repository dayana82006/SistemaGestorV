using System.Collections.Generic;

<<<<<<< HEAD
namespace SistemaGestorV.Domain.Ports
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> ObtenerTodos(); 
        T? ObtenerPorId(string id);       
        void Crear(T entity);
        void Actualizar(T entity);
        void Eliminar(string id);
    }
}
=======
namespace SistemaGestorV.Domain.Ports;

public interface IGenericRepository<T>
{
    IEnumerable<T> ObtenerTodos();
    T ObtenerPorId(int id);
    void Crear(T entity);
    void Actualizar(T entity);
    void Eliminar(int id);
}
>>>>>>> 40efe5fd4ebd427bbe8aab9888f57560a6d20a46
