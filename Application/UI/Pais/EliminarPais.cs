using System;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;
namespace SistemaGestorV.Application.UI.Pais;

public class EliminarPais
{
       private readonly PaisService _servicio;
       public EliminarPais(PaisService servicio)
       {
           _servicio = servicio;
       }
       public void Ejecutar()
       {
           Console.Write("Ingrese el ID del pais a eliminar: ");
           string id = Console.ReadLine()?.Trim() ?? string.Empty;

           if (string.IsNullOrWhiteSpace(id))
           {
               Console.WriteLine("❌ ID inválido.");
               return;
           }

           _servicio.EliminarPais(id);
       } 
}
