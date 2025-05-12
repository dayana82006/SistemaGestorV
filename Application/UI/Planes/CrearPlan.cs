using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGestorV.Application.UI
{
    public class CrearPlan
    {
        private readonly PlanServices _planServices;

        public CrearPlan(PlanServices planServices)
        {
            _planServices = planServices;
        }

        public void CrearPlanes()
        {
            Console.Clear();
            Console.WriteLine("===== CREAR NUEVO PLAN PROMOCIONAL =====");
            
            try
            {
                var plan = new Plan();
                
                // Solicitar nombre del plan
                Console.Write("\nNombre del plan: ");
                plan.Nombre = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(plan.Nombre))
                {
                    throw new ArgumentException("El nombre no puede estar vacío.");
                }
                
                // Solicitar fecha de inicio
                Console.Write("Fecha de inicio (dd/mm/aaaa): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaInicio))
                {
                    throw new ArgumentException("Fecha de inicio no válida.");
                }
                plan.FechaInicio = fechaInicio;
                
                // Solicitar fecha de fin
                Console.Write("Fecha de fin (dd/mm/aaaa): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaFin))
                {
                    throw new ArgumentException("Fecha de fin no válida.");
                }
                plan.FechaFin = fechaFin;
                
                if (plan.FechaInicio >= plan.FechaFin)
                {
                    throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin.");
                }
                
                // Solicitar porcentaje de descuento
                Console.Write("Porcentaje de descuento: ");
                if (!double.TryParse(Console.ReadLine(), out double descuento))
                {
                    throw new ArgumentException("El descuento debe ser un número válido.");
                }
                plan.dcto = descuento;
                
                if (plan.dcto < 0 || plan.dcto > 100)
                {
                    throw new ArgumentException("El porcentaje de descuento debe estar entre 0 y 100.");
                }
                
                // Obtener productos disponibles
                var productos = _planServices.ObtenerProductos();
                var productosSeleccionados = new List<string>();
                
                if (productos != null && productos.Count > 0)
                {
                    Console.WriteLine("\nProductos disponibles:");
                    foreach (var producto in productos)
                    {
                        Console.WriteLine($"ID: {producto.id} - Nombre: {producto.nombre}");
                    }
                    
                    // Solicitar IDs de productos
                    Console.Write("\nIngrese los IDs de los productos a asociar (separados por comas): ");
                    var idsInput = Console.ReadLine()?.Trim();
                    
                    if (!string.IsNullOrWhiteSpace(idsInput))
                    {
                        var ids = idsInput.Split(',').Select(id => id.Trim()).ToList();
                        
                        foreach (var id in ids)
                        {
                            var producto = productos.FirstOrDefault(p => 
                                string.Equals(p.id.ToString(), id, StringComparison.OrdinalIgnoreCase));
                            
                            if (producto != null)
                            {
                                // SOLUCIÓN CLAVE: Convertir el ID alfanumérico a numérico si es necesario
                                // Asumiendo que el ID real en BD es numérico aunque se muestre con prefijo
                                if (int.TryParse(ExtraerNumeros(producto.id.ToString()), out int idNumerico))
                                {
                                    productosSeleccionados.Add(idNumerico.ToString());
                                    Console.WriteLine($"- Producto '{producto.nombre}' agregado (ID numérico: {idNumerico})");
                                }
                                else
                                {
                                    Console.WriteLine($"- El ID '{producto.id}' no tiene formato convertible a número");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"- El producto con ID '{id}' no existe");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nNo hay productos disponibles para asociar.");
                }
                
                // Confirmar creación
                Console.Write("\n¿Crear este plan? (S/N): ");
                var respuesta = Console.ReadLine()?.Trim().ToUpper();
                if (respuesta != "S")
                {
                    Console.WriteLine("Operación cancelada.");
                    return;
                }
                
                // Crear el plan
                _planServices.CrearPlan(plan, productosSeleccionados);
                Console.WriteLine("Plan creado exitosamente!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear el plan: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Detalles: {ex.InnerException.Message}");
                }
            }
            
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private string ExtraerNumeros(string input)
        {
            // Extrae solo los dígitos numéricos de un string
            return new string(input.Where(char.IsDigit).ToArray());
        }
    }
}