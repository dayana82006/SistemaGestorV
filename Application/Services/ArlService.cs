using InventoryManagement.Domain.Entities;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System;
using System.Collections.Generic;

namespace SistemaGestorV.Application.Services
{
    public class ArlService
    {
        private readonly IArlRepository _repo;

        public ArlService(IArlRepository repo)
        {
            _repo = repo;
        }

        public ArlService()
        {
        }

        public void MostrarTodos()
        {
            var arl = _repo.ObtenerTodos();
            Console.WriteLine("\n--- Lista de arl ---");
            foreach (var a in arl)
            {
                Console.WriteLine($"ID: {a.id}, Nombre: {a.nombre}");
            }
            Console.WriteLine(new string('-', 60));
        }

        public void CrearArl(Arl arl)
        {
            _repo.Crear(arl);
        }

                public bool ActualizarArl(string id, string nombre)
        {
            var arl = _repo.ObtenerPorId(id);

            if (arl == null)
            {
                Console.WriteLine("❌ Arl no encontrada.");
                return false;
            }

            arl.nombre = nombre.Trim();
            _repo.Actualizar(arl);

            return true;
        }


        public void EliminarArl(string id)
        {
            var arl = _repo.ObtenerPorId(id);

            if (arl == null)
            {
                Console.WriteLine("Arl no encontrada.");
                return;
            }

            _repo.Eliminar(id);
            Console.WriteLine($"Arl ID: {id} eliminado con éxito.");
        }

        public IEnumerable<Arl> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public Arl ObtenerPorId(string id)
        {
            return _repo.ObtenerPorId(id);
        }
    }
}