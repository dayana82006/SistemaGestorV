using System.Collections.Generic;
using SistemaGestorV.Domain.Entities;

namespace SistemaGestorV.Domain.Ports
{
    public interface ITerceroRepository
    {
        IEnumerable<Tercero> ObtenerTodos();
        Tercero? ObtenerPorId(int id);
        void Crear(Tercero entity);
        void Actualizar(Tercero entity);
        void Eliminar(int id);
        IEnumerable<Tercero> ObtenerPorTipo(int tipoTerceroId);
        IEnumerable<Telefono> ObtenerTelefonosPorTercero(int terceroId);
        void AgregarTelefono(Telefono telefono);
        void EliminarTelefonosPorTercero(int terceroId);
    }
}