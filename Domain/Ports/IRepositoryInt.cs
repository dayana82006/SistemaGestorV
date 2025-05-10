
namespace SistemaGestorV.Domain.Ports
{
    public interface IRepositoryInt<T>
    {
        IEnumerable<T> ObtenerTodos(); 
        T? ObtenerPorId(int id);       
        void Crear(T entity);
        void Actualizar(T entity);
        void Eliminar(int id);
    }
}