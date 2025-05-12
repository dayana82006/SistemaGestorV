using System;
using System.Linq;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Eps
{
    public class CrearEps
    {
        private readonly EpsService _servicio;

        public CrearEps(EpsService servicio)
        {
            _servicio = servicio;
        }

        public void Ejecutar()
        {
            var eps = new SistemaGestorV.Domain.Entities.Eps();

            Console.Write("Id: ");
            string id = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!int.TryParse(id, out int idInt))
            {
                Console.WriteLine("❌ El ID debe ser un número entero válido.");
                return;
            }

            var epssExistentes = _servicio.ObtenerTodos();

            if (epssExistentes.Any(e => e.id == idInt))
            {
                Console.WriteLine("❌ Ya existe un EPS con ese ID.");
                return;
            }

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("❌ El nombre no puede estar vacío.");
                return;
            }

            if (epssExistentes.Any(e => e.nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("❌ Ya existe un EPS con ese nombre.");
                return;
            }

            eps.id = idInt;
            eps.nombre = nombre;

            _servicio.CrearEps(eps);
            Console.WriteLine("✅ EPS creado con éxito.");
        }
    }
}
