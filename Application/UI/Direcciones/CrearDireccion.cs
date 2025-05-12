using System;
using InventoryManagement.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Direcciones
{
    public class CrearDireccion
    {
        private readonly DireccionService _direccionServicio;
        private readonly CiudadService _ciudadServicio;

        public CrearDireccion(DireccionService direccionServicio, CiudadService ciudadServicio)
        {
            _direccionServicio = direccionServicio;
            _ciudadServicio = ciudadServicio;
        }

        public void Ejecutar()
        {
            Console.Clear();
            Console.WriteLine("\n--- Crear nueva dirección ---");

            // Solicitar ID personalizado para la dirección
            Console.Write("Ingrese ID de la dirección: ");
            string idInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int direccionId))
            {
                Console.WriteLine("❌ ID inválido. Debe ser un número entero.");
                return;
            }

            // Verificar si ya existe una dirección con ese ID
            var direccionExistente = _direccionServicio.ObtenerPorId(direccionId);
            if (direccionExistente != null)
            {
                Console.WriteLine("❌ Ese ID ya está en uso. No se puede crear la dirección.");
                return;
            }

            // Ciudad
            Console.Write("Ingrese ID de la ciudad: ");
            string ciudadIdInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(ciudadIdInput) || !int.TryParse(ciudadIdInput, out int ciudadId))
            {
                Console.WriteLine("❌ ID de ciudad inválido.");
                return;
            }
            var ciudad = _ciudadServicio.ObtenerPorId(ciudadId.ToString());
            if (ciudad == null)
            {
                Console.WriteLine("❌ Ciudad no encontrada. Regístrela primero.");
                return;
            }

            // Nombre de la calle
            Console.Write("Ingrese nombre de la calle: ");
            string nombreCalle = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(nombreCalle))
            {
                Console.WriteLine("❌ Nombre de calle inválido.");
                return;
            }

            // Número de la calle
            Console.Write("Ingrese número de la calle: ");
            string numeroCalleInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(numeroCalleInput))
            {
                Console.WriteLine("❌ Número de calle inválido.");
                return;
            }

            var nuevaDireccion = new Direccion
            {
                id = direccionId, 
                ciudadId = ciudadId,
                calleNombre = nombreCalle,
                calleNumero = numeroCalleInput,
            };

            try
            {
                _direccionServicio.CrearDireccion(nuevaDireccion);
                Console.WriteLine("✅ Dirección creada con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear la dirección: {ex.Message}");
            }
        }
    }
}
