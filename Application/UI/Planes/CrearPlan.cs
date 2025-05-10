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
                
                plan.Nombre = SolicitarTexto("\nNombre del plan: ", "El nombre no puede estar vacío.");
                
                plan.FechaInicio = SolicitarFecha("Fecha de inicio (dd/mm/aaaa): ", "Fecha de inicio no válida.");
                
                plan.FechaFin = SolicitarFecha("Fecha de fin (dd/mm/aaaa): ", "Fecha de fin no válida.");
                
                if (plan.FechaInicio >= plan.FechaFin)
                {
                    throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin.");
                }
                
                plan.dcto = SolicitarNumeroDecimal("Porcentaje de descuento: ", "El descuento debe ser un número válido.");
                
                if (plan.dcto < 0 || plan.dcto > 100)
                {
                    throw new ArgumentException("El porcentaje de descuento debe estar entre 0 y 100.");
                }
                
                var productos = _planServices.ObtenerProductos();
                var productosSeleccionados = new List<string>();
                
                if (productos != null && productos.Count > 0)
                {
                    Console.WriteLine("\nSeleccione los productos para este plan (ingrese los números separados por comas):");
                    
                    for (int i = 0; i < productos.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {productos[i].nombre}");
                    }
                    
                    productosSeleccionados = SolicitarProductos(productos);
                }
                else
                {
                    Console.WriteLine("\nNo hay productos disponibles para asociar.");
                }
                
                // Confirmar creación
                if (!ConfirmarOperacion("\n¿Crear este plan? (S/N): "))
                {
                    Console.WriteLine("Operación cancelada.");
                    return;
                }
                
                _planServices.CrearPlan(plan, productosSeleccionados);
                Console.WriteLine("\nPlan creado exitosamente!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear el plan: {ex.Message}");
            }
            
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        
        private string SolicitarTexto(string mensaje, string mensajeError)
        {
            string texto;
            do
            {
                Console.Write(mensaje);
                texto = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrWhiteSpace(texto))
                {
                    Console.WriteLine(mensajeError);
                }
            } while (string.IsNullOrWhiteSpace(texto));
            
            return texto;
        }

        private DateTime SolicitarFecha(string mensaje, string mensajeError)
        {
            DateTime fecha;
            bool esValido;
            
            do
            {
                Console.Write(mensaje);
                esValido = DateTime.TryParse(Console.ReadLine(), out fecha);
                
                if (!esValido)
                {
                    Console.WriteLine(mensajeError);
                }
            } while (!esValido);
            
            return fecha;
        }

        private double SolicitarNumeroDecimal(string mensaje, string mensajeError)
        {
            double numero;
            bool esValido;
            
            do
            {
                Console.Write(mensaje);
                esValido = double.TryParse(Console.ReadLine(), out numero);
                
                if (!esValido)
                {
                    Console.WriteLine(mensajeError);
                }
            } while (!esValido);
            
            return numero;
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

        private List<string> SolicitarProductos(List<SistemaGestorV.Domain.Entities.Producto> productos)        
        {
            var productosSeleccionados = new List<string>();
            bool seleccionValida = false;
            
            while (!seleccionValida)
            {
                Console.Write("\nSelección: ");
                var seleccion = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(seleccion))
                {
                    Console.WriteLine("No se ha seleccionado ningún producto.");
                    if (ConfirmarOperacion("¿Desea continuar sin seleccionar productos? (S/N): "))
                    {
                        seleccionValida = true;
                    }
                }
                else
                {
                    try
                    {
                        var indices = seleccion.Split(',')
                            .Select(s => s.Trim())
                            .Where(s => !string.IsNullOrWhiteSpace(s))
                            .ToList();
                            
                        bool todosNumerosValidos = true;
                        List<int> indicesNumericos = new List<int>();
                        
                        foreach (var indice in indices)
                        {
                            if (int.TryParse(indice, out int indiceNumerico))
                            {
                                if (indiceNumerico > 0 && indiceNumerico <= productos.Count)
                                {
                                    indicesNumericos.Add(indiceNumerico - 1);
                                }
                                else
                                {
                                    Console.WriteLine($"El índice {indiceNumerico} está fuera de rango. Debe estar entre 1 y {productos.Count}.");
                                    todosNumerosValidos = false;
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"'{indice}' no es un número válido.");
                                todosNumerosValidos = false;
                                break;
                            }
                        }
                        
                        if (todosNumerosValidos)
                        {
                            productosSeleccionados = indicesNumericos.Select(i => productos[i].nombre).ToList();
                            seleccionValida = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al procesar la selección: {ex.Message}");
                    }
                }
            }
            
            return productosSeleccionados;
        }
    }
}