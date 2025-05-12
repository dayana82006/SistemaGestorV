using System;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Ciudades
{
    public class ActualizarCiudad
    {
        private readonly RegionService _regionServicio;
        private readonly CiudadService _ciudadServicio;

        public ActualizarCiudad(RegionService regionServicio, CiudadService ciudadServicio)
        {
            _regionServicio = regionServicio;
            _ciudadServicio = ciudadServicio;
        }

        public void Ejecutar()
        {
            Console.Write("Id Ciudad a actualizar: ");
            string idInput = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
            {
                Console.WriteLine("❌ ID inválido. Debe ser un número entero.");
                return;
            }

            // Intentamos obtener la Ciudad a través del ID
            var ciudad = _ciudadServicio.ObtenerPorId(id.ToString());

            if (ciudad == null)
            {
                Console.WriteLine("❌ Ciudad no encontrada.");
                return;
            }

            // Actualización del nombre de la Ciudad
            Console.Write("Nuevo Nombre de la Ciudad: ");
            string nuevoNombre = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(nuevoNombre))
            {
                ciudad.nombre = nuevoNombre;

                // Actualizamos la Ciudad
                bool actualizado = _ciudadServicio.ActualizarCiudad(id.ToString(), nuevoNombre);

                if (actualizado)
                {
                    Console.WriteLine("✅ Ciudad actualizada con éxito.");
                }
            }
            else
            {
                Console.WriteLine("❌ Nombre inválido.");
            }

            // Actualización del región asociado a la región
            Console.Write("Nuevo ID de la región asociado (deja en blanco para no actualizar): ");
            string regionIdInput = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(regionIdInput))
            {
                if (int.TryParse(regionIdInput, out int regionId))
                {
                    var region = _regionServicio.ObtenerPorId(regionId.ToString());

                    if (region == null)
                    {
                        Console.WriteLine("❌ El ID de región ingresado no existe. Regístrelo primero.");
                        return;
                    }

                    ciudad.regionId = regionId;
                    _ciudadServicio.ActualizarCiudad(id.ToString(), nuevoNombre);  // Actualiza la región con el nuevo región

                    Console.WriteLine("✅ región actualizado con éxito.");
                }
                else
                {
                    Console.WriteLine("❌ ID de región inválido.");
                }
            }
        }
    }
}
