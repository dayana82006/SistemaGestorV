using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGestorV.Application.UI
{
    public class EliminarPlan
    {
        private readonly PlanServices _planServices;

        public EliminarPlan(PlanServices planServices)
        {
            _planServices = planServices;
        }

        public void EliminarPlanes()
        {
            Console.Clear();
            Console.WriteLine("===== ELIMINAR PLAN PROMOCIONAL =====");
            
            try
            {
                // Listar planes para seleccionar
                var planes = _planServices.ObtenerPlanes();
                
                if (planes == null || !planes.Any())
                {
                    Console.WriteLine("\nNo hay planes registrados para eliminar.");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                
                Console.WriteLine("\nSeleccione el plan a eliminar:");
                foreach (var plan in planes)
                {
                    Console.WriteLine($"{plan.Id}. {plan.Nombre} (Del {plan.FechaInicio:dd/MM/yyyy} al {plan.FechaFin:dd/MM/yyyy})");
                }
                
                int planId = SolicitarNumeroEntero("\nID del plan: ", "ID no válido.");
                
                var planAEliminar = planes.FirstOrDefault(p => p.Id == planId);
                if (planAEliminar == null)
                {
                    Console.WriteLine("Plan no encontrado.");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                
                // Mostrar datos del plan a eliminar
                Console.WriteLine("\nDatos del plan a eliminar:");
                Console.WriteLine($"Nombre: {planAEliminar.Nombre}");
                Console.WriteLine($"Fecha Inicio: {planAEliminar.FechaInicio:dd/MM/yyyy}");
                Console.WriteLine($"Fecha Fin: {planAEliminar.FechaFin:dd/MM/yyyy}");
                Console.WriteLine($"Descuento: {planAEliminar.dcto}%");
                Console.WriteLine("Productos asociados:");
                
                if (planAEliminar.ProductosAsociados != null && planAEliminar.ProductosAsociados.Any())
                {
                    foreach (var producto in planAEliminar.ProductosAsociados)
                    {
                        Console.WriteLine($"- {producto}");
                    }
                }
                else
                {
                    Console.WriteLine("- No hay productos asociados.");
                }
                
                // Confirmar eliminación
                if (!ConfirmarOperacion("\n¿Está seguro que desea eliminar este plan? (S/N): "))
                {
                    Console.WriteLine("Operación cancelada.");
                    return;
                }
                
                _planServices.EliminarPlan(planId);
                Console.WriteLine("\nPlan eliminado exitosamente!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar el plan: {ex.Message}");
            }
            
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private int SolicitarNumeroEntero(string mensaje, string mensajeError)
        {
            int numero;
            bool esValido;
            
            do
            {
                Console.Write(mensaje);
                esValido = int.TryParse(Console.ReadLine(), out numero);
                
                if (!esValido)
                {
                    Console.WriteLine(mensajeError);
                }
            } while (!esValido);
            
            return numero;
        }
        
        private bool ConfirmarOperacion(string mensaje)
        {
            Console.Write(mensaje);
            var respuesta = Console.ReadLine()?.Trim().ToUpper();
            return respuesta == "S";
        }
    }
}