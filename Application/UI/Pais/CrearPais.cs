using System;
using System.Linq;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Pais
{
    public class CrearPais
    {
        private readonly PaisService _servicio;

        public CrearPais(PaisService servicio)
        {
            _servicio = servicio;
        }

        public void Ejecutar()
        {
            var pais = new SistemaGestorV.Domain.Entities.Pais();

            Console.Write("Id: ");
            string id = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!int.TryParse(id, out int idInt))
            {
                Console.WriteLine("❌ El ID debe ser un número entero válido.");
                return;
            }

            var paisesExistentes = _servicio.ObtenerTodos();

            if (paisesExistentes.Any(p => p.id == idInt))
            {
                Console.WriteLine("❌ Ya existe un pais con ese ID.");
                return;
            }

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("❌ El nombre no puede estar vacío.");
                return;
            }

            if (paisesExistentes.Any(p => p.nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("❌ Ya existe un Pais con ese nombre.");
                return;
            }

            pais.id = idInt;
            pais.nombre = nombre;

            _servicio.CrearPais(pais);
            Console.WriteLine("✅ Pais creado con éxito.");
        }
    }
}
