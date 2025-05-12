using System;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;
namespace SistemaGestorV.Application.UI.Ciudades;

public class EliminarCiudad
{
       private readonly CiudadService _servicio;
       public EliminarCiudad(CiudadService servicio)
       {
           _servicio = servicio;
       }
       public void Ejecutar()
       {
           Console.Write("Ingrese el ID del Ciudad a eliminar: ");
           string id = Console.ReadLine()?.Trim() ?? string.Empty;

           if (string.IsNullOrWhiteSpace(id))
           {
               Console.WriteLine("❌ ID inválido.");
               return;
           }

           _servicio.EliminarCiudad(id);
       } 
}
