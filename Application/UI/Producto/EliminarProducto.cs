using System;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;
namespace SistemaGestorV.Application.UI.Producto;

public class EliminarProducto
{
       private readonly ProductoService _servicio;
       public EliminarProducto(ProductoService servicio)
       {
           _servicio = servicio;
       }
       public void Ejecutar()
       {
           Console.Write("Ingrese el ID del producto a eliminar: ");
           string id = Console.ReadLine()?.Trim() ?? string.Empty;

           if (string.IsNullOrWhiteSpace(id))
           {
               Console.WriteLine("❌ ID inválido.");
               return;
           }

           _servicio.EliminarProducto(id);
       } 
}
