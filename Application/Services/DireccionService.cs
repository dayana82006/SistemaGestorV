using InventoryManagement.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System;
using System.Collections.Generic;

namespace SistemaGestorV.Application.Services
{
    public class DireccionService
    {
        private readonly IDireccionRepository _repo;

        public DireccionService(IDireccionRepository repo)
        {
            _repo = repo;
        }

        public DireccionService()
        {
        }

        public void MostrarTodos()
        {
            var direcciones = _repo.ObtenerTodos();
            Console.WriteLine("\n--- Lista de Direcciones ---");
            foreach (var d in direcciones)
            {
                Console.WriteLine($"ID: {d.id}, Ciudad ID: {d.ciudadId}, Calle Nombre: {d.calleNombre}, Calle Número: {d.calleNumero}");
            }
            Console.WriteLine(new string('-', 60));
        }

        public void CrearDireccion(Direccion direccion)
        {
            _repo.Crear(direccion);
        }

        public bool ActualizarDireccion(int id, int ciudadId, string calleNombre, string calleNumero)
        {
            var direccion = _repo.ObtenerPorId(id.ToString());

            if (direccion == null)
            {
                Console.WriteLine("❌ Dirección no encontrada.");
                return false;
            }

            direccion.ciudadId = ciudadId;
            direccion.calleNombre = calleNombre;
            direccion.calleNumero = calleNumero;

            _repo.Actualizar(direccion);
            return true;
        }

        public void EliminarDireccion(int id)
        {
            var direccion = _repo.ObtenerPorId(id.ToString());

            if (direccion == null)
            {
                Console.WriteLine("❌ Dirección no encontrada.");
                return;
            }

            _repo.Eliminar(id.ToString());
            Console.WriteLine($"✅ Dirección ID: {id} eliminada con éxito.");
        }

        public IEnumerable<Direccion> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public Direccion ObtenerPorId(int id)
        {
            return _repo.ObtenerPorId(id.ToString());
        }
    }
}
