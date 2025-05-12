using System;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;
namespace SistemaGestorV.Application.UI.Direcciones;

public class EliminarDireccion
{
       private readonly DireccionService _servicio;
       public EliminarDireccion(DireccionService servicio)
       {
           _servicio = servicio;
       }
       public void Ejecutar()
       {
           Console.Write("Ingrese el ID de la Dirección a eliminar: ");
           string id = Console.ReadLine()?.Trim() ?? string.Empty;

           if (string.IsNullOrWhiteSpace(id))
           {
               Console.WriteLine("❌ ID inválido.");
               return;
           }

           if (int.TryParse(id, out int idInt))
           {
               _servicio.EliminarDireccion(idInt);
           }
           else
           {
               Console.WriteLine("❌ ID debe ser un número entero.");
           }
       } 
}
