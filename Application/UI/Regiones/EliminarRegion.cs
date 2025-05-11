using System;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;
namespace SistemaGestorV.Application.UI.Regiones;

public class EliminarRegion
{
       private readonly RegionService _servicio;
       public EliminarRegion(RegionService servicio)
       {
           _servicio = servicio;
       }
       public void Ejecutar()
       {
           Console.Write("Ingrese el ID de la Region a eliminar: ");
           string id = Console.ReadLine()?.Trim() ?? string.Empty;

           if (string.IsNullOrWhiteSpace(id))
           {
               Console.WriteLine("❌ ID inválido.");
               return;
           }

           _servicio.EliminarRegion(id);
       } 
}
