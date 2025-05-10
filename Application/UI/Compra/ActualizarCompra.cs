using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SistemaGestorV.Application.UI.Compra
{
    public class ActualizarCompra
    {
        private readonly CompraService _servicio;

        public ActualizarCompra(CompraService servicio)
        {
            _servicio = servicio;
        }

        public void Ejecutar()
        {
            Console.Write("🔁 Ingrese el ID de la compra a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int idCompra))
            {
                Console.WriteLine("❌ ID inválido.");
                return;
            }

            var compra = _servicio.ObtenerCompraPorId(idCompra);
            if (compra == null)
            {
                Console.WriteLine("❌ Compra no encontrada.");
                return;
            }

            Console.WriteLine($"🧾 Compra actual: ProveedorID = {compra.TerceroProvId}, EmpleadoID = {compra.TerceroEmpId}, Documento = {compra.DocCompra}");

            Console.Write("🧾 Nuevo ID del proveedor: ");
            if (!int.TryParse(Console.ReadLine(), out int nuevoProvId))
            {
                Console.WriteLine("❌ ID inválido.");
                return;
            }

            Console.Write("👤 Nuevo ID del empleado: ");
            if (!int.TryParse(Console.ReadLine(), out int nuevoEmpId))
            {
                Console.WriteLine("❌ ID inválido.");
                return;
            }

            Console.Write("📄 Nuevo documento de compra: ");
            var nuevoDoc = Console.ReadLine()?.Trim() ?? "";

            compra.TerceroProvId = nuevoProvId;
            compra.TerceroEmpId = nuevoEmpId;
            compra.DocCompra = nuevoDoc;
            compra.Fecha = DateTime.Now.Date;

            var nuevosDetalles = new List<DetalleCompra>();

            Console.Write("📦 Nueva cantidad de productos: ");
            if (!int.TryParse(Console.ReadLine(), out int cantidadProductos))
            {
                Console.WriteLine("❌ Cantidad inválida.");
                return;
            }

            for (int i = 0; i < cantidadProductos; i++)
            {
                var detalle = new DetalleCompra();

                Console.Write($"🆔 Producto ID {i + 1}: ");
                detalle.ProductoId = Console.ReadLine()?.Trim() ?? "";

                Console.Write($"🔢 Cantidad {i + 1}: ");
                if (!int.TryParse(Console.ReadLine(), out int cantidad))
                {
                    Console.WriteLine("❌ Cantidad inválida.");
                    return;
                }
                detalle.Cantidad = cantidad;

                Console.Write($"💲 Valor unitario {i + 1}: ");
                if (!double.TryParse(Console.ReadLine(), out double valor))
                {
                    Console.WriteLine("❌ Valor inválido.");
                    return;
                }
                detalle.Valor = valor;
                detalle.Fecha = DateTime.Now.Date;

                nuevosDetalles.Add(detalle);
            }

            compra.Detalles = nuevosDetalles;

            _servicio.EliminarCompra(compra.Id); // Limpia detalles anteriores y la compra
            _servicio.CrearCompra(compra);       // Inserta la compra y sus nuevos detalles

            Console.WriteLine("✅ Compra actualizada exitosamente.");
        }
    }
}
