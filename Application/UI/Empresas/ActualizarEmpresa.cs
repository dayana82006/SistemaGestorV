using System;
using SistemaGestorV.Application.Services;
using InventoryManagement.Domain.Entities;

namespace SistemaGestorV.Application.UI.Empresas
{
    public class ActualizarEmpresa
    {
        private readonly EmpresaService _empresaServicio;
        private readonly DireccionService _direccionServicio;

        public ActualizarEmpresa(EmpresaService empresaServicio, DireccionService direccionServicio)
        {
            _empresaServicio = empresaServicio;
            _direccionServicio = direccionServicio;
        }

        public void Ejecutar()
        {
            Console.Clear();
            Console.WriteLine("\n--- Actualizar Empresa ---");

            // ID de la empresa
            Console.Write("Ingrese ID de la empresa a actualizar: ");
            string idInput = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
            {
                Console.WriteLine("❌ ID inválido. Debe ser un número entero.");
                return;
            }

            var empresa = _empresaServicio.ObtenerPorId(id.ToString());

            if (empresa == null)
            {
                Console.WriteLine("❌ Empresa no encontrada.");
                return;
            }

            // ID de la dirección
            Console.Write("Nuevo ID de la dirección (deja en blanco para no actualizar): ");
            string direccionIdInput = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(direccionIdInput))
            {
                if (int.TryParse(direccionIdInput, out int direccionId))
                {
                    var direccion = _direccionServicio.ObtenerPorId(direccionId);
                    if (direccion == null)
                    {
                        Console.WriteLine("❌ Dirección no encontrada. Regístrela primero.");
                        return;
                    }

                    empresa.direccionId = direccionId;
                }
                else
                {
                    Console.WriteLine("❌ ID de dirección inválido.");
                    return;
                }
            }

            // Nombre de la empresa
            Console.Write("Nuevo nombre de la empresa (deja en blanco para no actualizar): ");
            string nombreEmpresaInput = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(nombreEmpresaInput))
            {
                empresa.nombre = nombreEmpresaInput;
            }

            bool actualizado = _empresaServicio.ActualizarEmpresa(
                empresa.id,
                empresa.direccionId,
                empresa.nombre,
                empresa.fechaReg
            );

            if (actualizado)
            {
                Console.WriteLine("✅ Empresa actualizada con éxito.");
            }
            else
            {
                Console.WriteLine("❌ No se pudo actualizar la empresa.");
            }
        }
    }
}
