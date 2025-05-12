using System;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;
namespace SistemaGestorV.Application.UI.Empresas;

public class EliminarEmpresa
{
       private readonly EmpresaService _servicio;
       public EliminarEmpresa(EmpresaService servicio)
       {
           _servicio = servicio;
       }
    
         public void Ejecutar()
         {
              Console.WriteLine("\n--- Eliminar Empresa ---");
              Console.Write("Ingrese el ID de la empresa a eliminar: ");
              var id = Console.ReadLine();
    
              if (string.IsNullOrEmpty(id))
              {
                Console.WriteLine("❌ ID no válido.");
                return;
              }
    
              var empresa = _servicio.ObtenerPorId(id);
    
              if (empresa == null)
              {
                Console.WriteLine("❌ Empresa no encontrada.");
                return;
              }
    
              _servicio.EliminarEmpresa(id);
              Console.WriteLine($"✅ Empresa con ID {id} eliminada correctamente.");
         }
}
