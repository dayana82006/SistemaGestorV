using System;
using System.Linq;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Arl
{
    public class CrearArl
    {
        private readonly ArlService _servicio;

        public CrearArl(ArlService servicio)
        {
            _servicio = servicio;
        }

        public void Ejecutar()
        {
            var arl = new SistemaGestorV.Domain.Entities.Arl();

            Console.Write("Id: ");
            string id = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!int.TryParse(id, out int idInt))
            {
                Console.WriteLine("❌ El ID debe ser un número entero válido.");
                return;
            }

            var arlsExistentes = _servicio.ObtenerTodos();

            if (arlsExistentes.Any(a => a.id == idInt))
            {
                Console.WriteLine("❌ Ya existe un ARL con ese ID.");
                return;
            }

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("❌ El nombre no puede estar vacío.");
                return;
            }

            if (arlsExistentes.Any(e => e.nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("❌ Ya existe un ARL con ese nombre.");
                return;
            }

            arl.id = idInt;
            arl.nombre = nombre;

            _servicio.CrearArl(arl);
            Console.WriteLine("✅ ARL creado con éxito.");
        }
    }
}
