using System;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;


namespace SistemaGestorV.Application.UI.Producto
{
    public class CrearProducto
    {
        private readonly ProductoService _servicio;

        public CrearProducto(ProductoService servicio)
        {
            _servicio = servicio;
        }

        public void Ejecutar()
        {
           var producto = new SistemaGestorV.Domain.Entities.Producto();

            Console.Write("Id: ");
            string id = Console.ReadLine()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("❌ ID inválido.");
                return;
            }
        
            producto.id = id;
            Console.Write("Nombre: ");
            producto.nombre = Console.ReadLine()?.Trim() ?? string.Empty;

            Console.Write("Stock: ");
            if (!int.TryParse(Console.ReadLine(), out int stock))
            {
                Console.WriteLine("❌ Stock inválido.");
                return;
            }

            Console.Write("Stock mínimo: ");
            if (!int.TryParse(Console.ReadLine(), out int stockMin))
            {
                Console.WriteLine("❌ Stock mínimo inválido.");
                return;
            }

            Console.Write("Stock máximo: ");
            if (!int.TryParse(Console.ReadLine(), out int stockMax))
            {
                Console.WriteLine("❌ Stock máximo inválido.");
                return;
            }

            Console.Write("Código de barras: ");
            producto.barcode = Console.ReadLine()?.Trim() ?? string.Empty;

            producto.id = id;
            producto.stock = stock;
            producto.stockMin = stockMin;
            producto.stockMax = stockMax;
            producto.createdAt = DateTime.Now.Date;
            producto.updatedAt = DateTime.Now.Date;
            producto.barcode = producto.barcode;

            _servicio.CrearProducto(producto);
            Console.WriteLine("✅ Producto creado con éxito.");
        }
    }
}
