namespace SistemaGestorV.Application.UI.Compra
{
    using System;
    using SistemaGestorV.Domain.Entities;
    using SistemaGestorV.Application.Services;
    using System.Collections.Generic;

    public class CrearCompra
    {
        private readonly CompraService _servicio;

        public CrearCompra(CompraService servicio)
        {
            _servicio = servicio;
        }
public void Ejecutar()
{
    var compra = new SistemaGestorV.Domain.Entities.Compra();

    Console.Write("🧾 ID del proveedor: ");
    if (!int.TryParse(Console.ReadLine(), out int terceroProvId))
    {
        Console.WriteLine("❌ ID inválido.");
        return;
    }

    Console.Write("👤 ID del empleado: ");
    if (!int.TryParse(Console.ReadLine(), out int terceroEmpId))
    {
        Console.WriteLine("❌ ID inválido.");
        return;
    }

    Console.Write("📄 Documento de compra: ");
    var docCompra = Console.ReadLine()?.Trim() ?? "";

    compra.TerceroProvId = terceroProvId;
    compra.TerceroEmpId = terceroEmpId;
    compra.DocCompra = docCompra;
    compra.Fecha = DateTime.Now.Date;

    var detalles = new List<DetalleCompra>();
    Console.Write("📦 Cantidad de productos: ");
    if (!int.TryParse(Console.ReadLine(), out int cantidadProductos))
    {
        Console.WriteLine("❌ Cantidad inválida.");
        return;
    }

    for (int i = 0; i < cantidadProductos; i++)
    {
        var detalleCompra = new DetalleCompra();

        Console.Write($"🆔 Producto ID {i + 1}: ");
        detalleCompra.ProductoId = Console.ReadLine()?.Trim() ?? "";

        Console.Write($"🔢 Cantidad {i + 1}: ");
        if (!int.TryParse(Console.ReadLine(), out int cantidad))
        {
            Console.WriteLine("❌ Cantidad inválida.");
            return;
        }
        detalleCompra.Cantidad = cantidad;

        Console.Write($"💲 Valor unitario {i + 1}: ");
        if (!double.TryParse(Console.ReadLine(), out double valor))
        {
            Console.WriteLine("❌ Valor inválido.");
            return;
        }
        detalleCompra.Valor = valor;
        detalleCompra.Fecha = DateTime.Now.Date;

        detalles.Add(detalleCompra);
    }

    compra.Detalles = detalles;

    _servicio.CrearCompra(compra);
    Console.WriteLine("✅ Compra registrada exitosamente.");
}


    }
}
