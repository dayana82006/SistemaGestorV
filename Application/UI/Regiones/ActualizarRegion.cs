using System;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Regiones
{
    public class ActualizarRegion
    {
        private readonly RegionService _regionServicio;
        private readonly PaisService _paisServicio;

        public ActualizarRegion(RegionService regionServicio, PaisService paisServicio)
        {
            _regionServicio = regionServicio;
            _paisServicio = paisServicio;
        }

        public void Ejecutar()
        {
            Console.Write("Id Región a actualizar: ");
            string idInput = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
            {
                Console.WriteLine("❌ ID inválido. Debe ser un número entero.");
                return;
            }

            // Intentamos obtener la región a través del ID
            var region = _regionServicio.ObtenerPorId(id.ToString());

            if (region == null)
            {
                Console.WriteLine("❌ Región no encontrada.");
                return;
            }

            // Actualización del nombre de la región
            Console.Write("Nuevo Nombre de la Región: ");
            string nuevoNombre = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(nuevoNombre))
            {
                region.nombre = nuevoNombre;

                // Actualizamos la región
                bool actualizado = _regionServicio.ActualizarRegion(id.ToString(), nuevoNombre);

                if (actualizado)
                {
                    Console.WriteLine("✅ Región actualizada con éxito.");
                }
            }
            else
            {
                Console.WriteLine("❌ Nombre inválido.");
            }

            // Actualización del país asociado a la región
            Console.Write("Nuevo ID del país asociado (deja en blanco para no actualizar): ");
            string paisIdInput = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(paisIdInput))
            {
                if (int.TryParse(paisIdInput, out int paisId))
                {
                    var pais = _paisServicio.ObtenerPorId(paisId.ToString());

                    if (pais == null)
                    {
                        Console.WriteLine("❌ El ID de país ingresado no existe. Regístrelo primero.");
                        return;
                    }

                    region.paisId = paisId;
                    _regionServicio.ActualizarRegion(id.ToString(), nuevoNombre);  // Actualiza la región con el nuevo país

                    Console.WriteLine("✅ País actualizado con éxito.");
                }
                else
                {
                    Console.WriteLine("❌ ID de país inválido.");
                }
            }
        }
    }
}
