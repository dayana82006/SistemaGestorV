using System;
using InventoryManagement.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Empresas
{
    public class CrearEmpresa
    {
        private readonly EmpresaService _EmpresaServicio;
        private readonly DireccionService _DireccionServicio;

        public CrearEmpresa(EmpresaService empresaServicio, DireccionService direccionServicio)
        {
            _EmpresaServicio = empresaServicio;
            _DireccionServicio = direccionServicio;
        }

        public void Ejecutar()
        {
            Console.Clear();
            Console.WriteLine("\n--- Crear nueva empresa ---");

            // Solicitar ID de la empresa (string alfanumérico)
            Console.Write("Ingrese ID de la empresa (puede contener letras y números): ");
            string empresaId = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(empresaId))
            {
                Console.WriteLine("❌ ID de empresa inválido.");
                return;
            }

            // Verificar si ya existe una empresa con ese ID
            var empresaExistente = _EmpresaServicio.ObtenerPorId(empresaId);
            if (empresaExistente != null)
            {
                Console.WriteLine("❌ Ya existe una empresa con este ID. Elija otro ID.");
                return;
            }

            // Solicitar ID de dirección
            Console.Write("Ingrese ID de la dirección: ");
            string direccionIdInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(direccionIdInput) || !int.TryParse(direccionIdInput, out int direccionId))
            {
                Console.WriteLine("❌ ID de dirección inválido.");
                return;
            }

            var direccion = _DireccionServicio.ObtenerPorId(direccionId);
            if (direccion == null)
            {
                Console.WriteLine("❌ Dirección no encontrada. Regístrela primero.");
                return;
            }

            // Solicitar nombre de empresa
            Console.Write("Ingrese nombre de la empresa: ");
            string nombreEmpresa = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(nombreEmpresa))
            {
                Console.WriteLine("❌ Nombre de empresa inválido.");
                return;
            }

            var nuevaEmpresa = new Empresa
            {
                id = empresaId, // Asignar el ID de empresa ingresado por el usuario
                direccionId = direccionId,
                nombre = nombreEmpresa,
                fechaReg = DateTime.Now,
            };

            try
            {
                _EmpresaServicio.CrearEmpresa(nuevaEmpresa);
                Console.WriteLine("✅ Empresa creada con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear la empresa: {ex.Message}");
            }
        }
    }
}
