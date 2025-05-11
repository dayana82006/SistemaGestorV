using System;
using System.Linq;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Regioneses
{
    public class CrearRegion
    {
        private readonly RegionService _regionServicio;
        private readonly PaisService _paisServicio;

        public CrearRegion(RegionService regionServicio, PaisService paisServicio)
        {
            _regionServicio = regionServicio;
            _paisServicio = paisServicio;
        }

        public void Ejecutar()
        {
            var region = new Region();

            Console.Write("Id: ");
            string idInput = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!int.TryParse(idInput, out int id))
            {
                Console.WriteLine("❌ El ID debe ser un número entero válido.");
                return;
            }

            if (_regionServicio.ObtenerTodos().Any(r => r.id == id))
            {
                Console.WriteLine("❌ Ya existe una región con ese ID.");
                return;
            }

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("❌ El nombre no puede estar vacío.");
                return;
            }

            if (_regionServicio.ObtenerTodos().Any(r => r.nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("❌ Ya existe una región con ese nombre.");
                return;
            }

            Console.Write("ID del país asociado: ");
            string paisIdInput = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!int.TryParse(paisIdInput, out int paisId))
            {
                Console.WriteLine("❌ El ID del país debe ser un número válido.");
                return;
            }

            // ✅ Conversión corregida
            var pais = _paisServicio.ObtenerPorId(paisId.ToString());

            if (pais == null)
            {
                Console.WriteLine("❌ Ese ID no existe en país. Regístrelo primero.");
                return;
            }

            region.id = id;
            region.nombre = nombre;
            region.paisId = paisId;

            _regionServicio.CrearRegion(region);
            Console.WriteLine("✅ Región creada con éxito.");
        }
    }
}
