using System;
using SistemaGestorV.Domain.Entities;

namespace SistemaGestorV.Domain.Ports
{
 public interface IPlanesRepository
    {
        IEnumerable<Plan> ObtenerTodos();
        Plan ObtenerPorId(string id);
        Plan ObtenerPorId(int id);
        void Crear(Plan plan);
        void Actualizar(Plan plan);
        void Eliminar(string id);
        void Eliminar(int id);
    }
}
