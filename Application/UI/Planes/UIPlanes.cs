using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Factory;
using SistemaGestorV.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGestorV.Application.UI
{
    public class UIPlanes
    {
        private readonly PlanServices _servicio;
        private readonly ProductoService _productoService;

        public UIPlanes(IDbFactory factory)
        {
            _productoService = new ProductoService(factory.CrearProductoRepository());
            _servicio = new PlanServices(factory.CrearPlanesRepository(), _productoService);
        }

        public void GestionarPlanes()
        {
            bool volver = false;
            
            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("===== GESTIÓN DE PLANES PROMOCIONALES =====");
                Console.WriteLine("1. Listar planes");
                Console.WriteLine("2. Crear nuevo plan");
                Console.WriteLine("3. Actualizar plan");
                Console.WriteLine("4. Eliminar plan");
                Console.WriteLine("0. Volver al menú principal");
                Console.Write("\nSeleccione una opción: ");
                
                var opcion = Console.ReadLine();
                
                switch (opcion)
                {
                    case "1":
                        ListarPlanes();
                        break;
                    case "2":
                        var crearPlan = new CrearPlan(_servicio);
                        crearPlan.CrearPlanes();
                        break;
                    case "3":
                        var actualizarPlan = new ActualizarPlan(_servicio);
                        actualizarPlan.ActualizarPlanes();
                        break;
                    case "4":
                        var eliminarPlan = new EliminarPlan(_servicio);
                        eliminarPlan.EliminarPlanes();
                        break;
                    case "0":
                        volver = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ListarPlanes()
        {
            Console.Clear();
            Console.WriteLine("===== LISTADO DE PLANES PROMOCIONALES =====");
            
            try
            {
                var planes = _servicio.ObtenerPlanes();
                
                if (!planes.Any())
                {
                    Console.WriteLine("\nNo hay planes registrados.");
                }
                else
                {
                    foreach (var plan in planes)
                    {
                        Console.WriteLine($"Nombre: {plan.Nombre}");
                        Console.WriteLine($"Fecha Inicio: {plan.FechaInicio:dd/MM/yyyy}");
                        Console.WriteLine($"Fecha Fin: {plan.FechaFin:dd/MM/yyyy}");
                        Console.WriteLine($"Descuento: {plan.Descuento}%");
                        Console.WriteLine("Productos asociados:");
                        
                        if (plan.ProductosAsociados != null && plan.ProductosAsociados.Any())
                        {
                            foreach (var producto in plan.ProductosAsociados)
                            {
                                Console.WriteLine($"- {producto}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("- No hay productos asociados.");
                        }
                        
                        Console.WriteLine(new string('-', 50));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al obtener los planes: {ex.Message}");
            }
            
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}