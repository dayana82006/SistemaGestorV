using InventoryManagement.Domain.Entities;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System;
using System.Collections.Generic;

namespace SistemaGestorV.Application.Services
{
    public class CiudadService
    {
        private readonly ICiudadRepository _repo;

        public CiudadService(ICiudadRepository repo)
        {
            _repo = repo;
        }

        public CiudadService()
        {
        }

        public void MostrarTodos()
        {
            var ciudad = _repo.ObtenerTodos();
            Console.WriteLine("\n--- Lista de ciudad ---");
            foreach (var c in ciudad)
            {
                Console.WriteLine($"ID: {c.id}, Nombre: {c.nombre}");
            }
            Console.WriteLine(new string('-', 60));
        }

        public void CrearCiudad(Ciudad ciudad)
        {
            _repo.Crear(ciudad);
        }

                public bool ActualizarCiudad(string id, string nombre)
        {
            var ciudad = _repo.ObtenerPorId(id);

            if (ciudad == null)
            {
                Console.WriteLine("❌ Ciudad no encontrada.");
                return false;
            }

            ciudad.nombre = nombre.Trim();
            _repo.Actualizar(ciudad);

            return true;
        }


        public void EliminarCiudad(string id)
        {
            var ciudad = _repo.ObtenerPorId(id);

            if (ciudad == null)
            {
                Console.WriteLine("Ciudad no encontrada.");
                return;
            }

            _repo.Eliminar(id);
            Console.WriteLine($"Ciudad ID: {id} eliminado con éxito.");
        }

        public IEnumerable<Ciudad> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public Ciudad ObtenerPorId(string id)
        {
            return _repo.ObtenerPorId(id);
        }
    }
}