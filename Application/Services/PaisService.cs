using InventoryManagement.Domain.Entities;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System;
using System.Collections.Generic;

namespace SistemaGestorV.Application.Services
{
    public class PaisService
    {
        private readonly IPaisRepository _repo;

        public PaisService(IPaisRepository repo)
        {
            _repo = repo;
        }

        public PaisService()
        {
        }

        public void MostrarTodos()
        {
            var pais = _repo.ObtenerTodos();
            Console.WriteLine("\n--- Lista de paises ---");
            foreach (var p in pais)
            {
                Console.WriteLine($"ID: {p.id}, Nombre: {p.nombre}");
            }
            Console.WriteLine(new string('-', 60));
        }

        public void CrearPais(Pais pais)
        {
            _repo.Crear(pais);
        }

                public bool ActualizarPais(string id, string nombre)
        {
            var pais = _repo.ObtenerPorId(id);

            if (pais == null)
            {
                Console.WriteLine("❌ Pais no encontrada.");
                return false;
            }

            pais.nombre = nombre.Trim();
            _repo.Actualizar(pais);

            return true;
        }


        public void EliminarPais(string id)
        {
            var pais = _repo.ObtenerPorId(id);

            if (pais == null)
            {
                Console.WriteLine("Pais no encontrada.");
                return;
            }

            _repo.Eliminar(id);
            Console.WriteLine($"Pais ID: {id} eliminado con éxito.");
        }

        public IEnumerable<Pais> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public Pais ObtenerPorId(string id)
        {
            return _repo.ObtenerPorId(id);
        }
        
    }
}