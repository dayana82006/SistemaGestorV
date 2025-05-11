using System;
using System.Linq;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Ciudad
{
    public class CrearCiudad
    {
        private readonly CiudadService _servicio;

        public CrearCiudad(CiudadService servicio)
        {
            _servicio = servicio;
        }

        public void Ejecutar()
        {
            var ciudad = new SistemaGestorV.Domain.Entities.Ciudad();

            Console.Write("Id: ");
            string id = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!int.TryParse(id, out int idInt))
            {
                Console.WriteLine("❌ El ID debe ser un número entero válido.");
                return;
            }

            var ciudadesExistentes = _servicio.ObtenerTodos();

            if (ciudadesExistentes.Any(c => c.id == idInt))
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

            if (ciudadesExistentes.Any(c => c.nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("❌ Ya existe una ciudad con ese nombre.");
                return;
            }

            ciudad.id = idInt;
            ciudad.nombre = nombre;

            _servicio.CrearCiudad(ciudad);
            Console.WriteLine("✅ Ciudad creado con éxito.");
        }
    }
}
