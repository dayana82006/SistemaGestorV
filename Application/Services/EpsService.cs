using InventoryManagement.Domain.Entities;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System;
using System.Collections.Generic;

namespace SistemaGestorV.Application.Services
{
    public class EpsService
    {
        private readonly IEpsRepository _repo;

        public EpsService(IEpsRepository repo)
        {
            _repo = repo;
        }

        public EpsService()
        {
        }

        public void MostrarTodos()
        {
            var eps = _repo.ObtenerTodos();
            Console.WriteLine("\n--- Lista de eps ---");
            foreach (var e in eps)
            {
                Console.WriteLine($"ID: {e.id}, Nombre: {e.nombre}");
            }
            Console.WriteLine(new string('-', 60));
        }

        public void CrearEps(Eps eps)
        {
            _repo.Crear(eps);
        }

                public bool ActualizarEps(string id, string nombre)
        {
            var eps = _repo.ObtenerPorId(id);

            if (eps == null)
            {
                Console.WriteLine("❌ Eps no encontrada.");
                return false;
            }

            eps.nombre = nombre.Trim();
            _repo.Actualizar(eps);

            return true;
        }


        public void EliminarEps(string id)
        {
            var eps = _repo.ObtenerPorId(id);

            if (eps == null)
            {
                Console.WriteLine("Eps no encontrada.");
                return;
            }

            _repo.Eliminar(id);
            Console.WriteLine($"Eps ID: {id} eliminado con éxito.");
        }

        public IEnumerable<Eps> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public Eps ObtenerPorId(string id)
        {
            return _repo.ObtenerPorId(id);
        }
    }
}