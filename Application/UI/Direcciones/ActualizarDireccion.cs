using System;
using SistemaGestorV.Application.Services;
using InventoryManagement.Domain.Entities;

namespace SistemaGestorV.Application.UI.Direcciones
{
    public class ActualizarDireccion
    {
        private readonly DireccionService _direccionServicio;
        private readonly CiudadService _ciudadServicio;

        public ActualizarDireccion(DireccionService direccionServicio, CiudadService ciudadServicio)
        {
            _direccionServicio = direccionServicio;
            _ciudadServicio = ciudadServicio;
        }

        public void Ejecutar()
        {
            Console.Write("Id de la Dirección a actualizar: ");
            string idInput = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
            {
                Console.WriteLine("❌ ID inválido. Debe ser un número entero.");
                return;
            }

            var direccion = _direccionServicio.ObtenerPorId(id);

            if (direccion == null)
            {
                Console.WriteLine("❌ Dirección no encontrada.");
                return;
            }

            Console.Write("Nuevo ID de Ciudad (deja en blanco para no actualizar): ");
            string nuevaCiudadId = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(nuevaCiudadId))
            {
                if (int.TryParse(nuevaCiudadId, out int ciudadId))
                {
                    var ciudad = _ciudadServicio.ObtenerPorId(ciudadId.ToString());
                    if (ciudad == null)
                    {
                        Console.WriteLine("❌ Ciudad no encontrada. Regístrela primero.");
                        return;
                    }

                    direccion.ciudadId = ciudadId;
                }
                else
                {
                    Console.WriteLine("❌ ID de ciudad inválido.");
                    return;
                }
            }

            Console.Write("Nuevo nombre de calle (deja en blanco para no actualizar): ");
            string calleNombreInput = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(calleNombreInput))
            {
                direccion.calleNombre = calleNombreInput;
            }

            Console.Write("Nuevo número de calle (deja en blanco para no actualizar): ");
            string calleNumeroInput = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(calleNumeroInput))
            {
                direccion.calleNumero = calleNumeroInput;
            }

            bool actualizado = _direccionServicio.ActualizarDireccion(
                direccion.id,
                direccion.ciudadId,
                direccion.calleNombre,
                direccion.calleNumero
            );

            if (actualizado)
            {
                Console.WriteLine("✅ Dirección actualizada con éxito.");
            }
            else
            {
                Console.WriteLine("❌ No se pudo actualizar la dirección.");
            }
        }
    }
}
