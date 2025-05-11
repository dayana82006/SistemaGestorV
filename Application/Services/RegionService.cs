using InventoryManagement.Domain.Entities;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System;
using System.Collections.Generic;

namespace SistemaGestorV.Application.Services
{
    public class RegionService
    {
        private readonly IRegionRepository _repo;

        public RegionService(IRegionRepository repo)
        {
            _repo = repo;
        }

        public RegionService()
        {
        }

        public void MostrarTodos()
        {
            var region = _repo.ObtenerTodos();
            Console.WriteLine("\n--- Lista de region ---");
            foreach (var r in region)
            {
                Console.WriteLine($"ID: {r.id}, Nombre: {r.nombre}");
            }
            Console.WriteLine(new string('-', 60));
        }

        public void CrearRegion(Region region)
        {
            _repo.Crear(region);
        }

                public bool ActualizarRegion(string id, string nombre)
        {
            var region = _repo.ObtenerPorId(id);

            if (region == null)
            {
                Console.WriteLine("❌ Región no encontrada.");
                return false;
            }

            region.nombre = nombre.Trim();
            _repo.Actualizar(region);

            return true;
        }


        public void EliminarRegion(string id)
        {
            var region = _repo.ObtenerPorId(id);

            if (region == null)
            {
                Console.WriteLine("Región no encontrada.");
                return;
            }

            _repo.Eliminar(id);
            Console.WriteLine($"Región ID: {id} eliminado con éxito.");
        }

        public IEnumerable<Region> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public Region ObtenerPorId(string id)
        {
            return _repo.ObtenerPorId(id);
        }
    }
}