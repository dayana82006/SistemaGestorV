using System;
using System.Linq;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Ciudades
{
    public class CrearCiudad
    {
        private readonly CiudadService _ciudadServicio;
        private readonly RegionService _regionServicio;

        public CrearCiudad(CiudadService ciudadServicio, RegionService regionServicio)
        {
            _ciudadServicio = ciudadServicio;
            _regionServicio = regionServicio;
        }

        public void Ejecutar()
        {
            var ciudad = new Ciudad();

            Console.Write("Id: ");
            string idInput = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!int.TryParse(idInput, out int id))
            {
                Console.WriteLine("❌ El ID debe ser un número entero válido.");
                return;
            }

            if (_ciudadServicio.ObtenerTodos().Any(c => c.id == id))
            {
                Console.WriteLine("❌ Ya existe una ciudad con ese ID.");
                return;
            }

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("❌ El nombre no puede estar vacío.");
                return;
            }

            if (_ciudadServicio.ObtenerTodos().Any(c => c.nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("❌ Ya existe una ciudad con ese nombre.");
                return;
            }

            Console.Write("ID de la región asociada: ");
            string regionIdInput = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!int.TryParse(regionIdInput, out int regionId))
            {
                Console.WriteLine("❌ El ID de la región debe ser un número válido.");
                return;
            }

            var region = _regionServicio.ObtenerPorId(regionId.ToString());

            if (region == null)
            {
                Console.WriteLine("❌ Ese ID no existe en región. Regístrelo primero.");
                return;
            }

            ciudad.id = id;
            ciudad.nombre = nombre;
            ciudad.regionId = regionId;

            _ciudadServicio.CrearCiudad(ciudad);
            Console.WriteLine("✅ ciudad creada con éxito.");
        }
    }
}
