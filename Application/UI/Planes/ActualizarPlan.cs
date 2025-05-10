using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGestorV.Application.UI
{
    public class ActualizarPlan
    {
        private readonly PlanServices _planServices;

        public ActualizarPlan(PlanServices planServices)
        {
            _planServices = planServices;
        }

        public void ActualizarPlanes()
        {
            Console.Clear();
            Console.WriteLine("===== ACTUALIZAR PLAN PROMOCIONAL =====");
            
            try
            {
                // Listar planes para seleccionar
                var planes = _planServices.ObtenerPlanes();
                
                if (planes == null || !planes.Any())
                {
                    Console.WriteLine("\nNo hay planes registrados para actualizar.");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                
                Console.WriteLine("\nSeleccione el plan a actualizar:");
                foreach (var plan in planes)
                {
                    Console.WriteLine($"{plan.Id}. {plan.Nombre} (Del {plan.FechaInicio:dd/MM/yyyy} al {plan.FechaFin:dd/MM/yyyy})");
                }
                
                int planId = SolicitarNumeroEntero("\nID del plan: ", "ID no válido.");
                
                var planActual = planes.FirstOrDefault(p => p.Id == planId);
                if (planActual == null)
                {
                    Console.WriteLine("Plan no encontrado.");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                
                // Mostrar datos actuales
                Console.WriteLine("\nDatos actuales del plan:");
                Console.WriteLine($"Nombre: {planActual.Nombre}");
                Console.WriteLine($"Fecha Inicio: {planActual.FechaInicio:dd/MM/yyyy}");
                Console.WriteLine($"Fecha Fin: {planActual.FechaFin:dd/MM/yyyy}");
                Console.WriteLine($"Descuento: {planActual.dcto}%");
                Console.WriteLine("Productos asociados:");
                
                if (planActual.ProductosAsociados != null && planActual.ProductosAsociados.Any())
                {
                    foreach (var producto in planActual.ProductosAsociados)
                    {
                        Console.WriteLine($"- {producto}");
                    }
                }
                else
                {
                    Console.WriteLine("- No hay productos asociados.");
                }
                
                // Solicitar nuevos datos
                Console.WriteLine("\nIngrese los nuevos datos (deje en blanco para mantener el valor actual):");
                
                planActual.Nombre = SolicitarTextoOpcional($"Nombre [{planActual.Nombre}]: ", planActual.Nombre);
                
                planActual.FechaInicio = SolicitarFechaOpcional($"Fecha de inicio [{planActual.FechaInicio:dd/MM/yyyy}]: ", planActual.FechaInicio);
                
                planActual.FechaFin = SolicitarFechaOpcional($"Fecha de fin [{planActual.FechaFin:dd/MM/yyyy}]: ", planActual.FechaFin);
                
                planActual.dcto = SolicitarNumeroDecimalOpcional($"Porcentaje de descuento [{planActual.dcto}]: ", planActual.dcto);
                
                // Validar fechas
                if (planActual.FechaInicio >= planActual.FechaFin)
                {
                    throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin.");
                }
                
                // Validar descuento
                if (planActual.dcto < 0 || planActual.dcto > 100)
                {
                    throw new ArgumentException("El porcentaje de descuento debe estar entre 0 y 100.");
                }
                
                // Mostrar lista de productos para actualizar
                var productos = _planServices.ObtenerProductos();
                var productosSeleccionados = new List<string>();
                
                if (productos != null && productos.Count > 0)
                {
                    Console.WriteLine("\nSeleccione los nuevos productos para este plan (ingrese los números separados por comas):");
                    Console.WriteLine("Nota: Esto reemplazará todos los productos actualmente asociados.");
                    
                    for (int i = 0; i < productos.Count; i++)
                    {
                        var estaSeleccionado = planActual.ProductosAsociados != null && 
                                              planActual.ProductosAsociados.Any(p => p == productos[i].nombre);
                        Console.WriteLine($"{i + 1}. [{(estaSeleccionado ? "X" : " ")}] {productos[i].nombre}");
                    }
                    
                    productosSeleccionados = SolicitarProductos(productos);
                }
                else
                {
                    Console.WriteLine("\nNo hay productos disponibles para asociar.");
                }
                
                // Confirmar actualización
                if (!ConfirmarOperacion("\n¿Actualizar este plan? (S/N): "))
                {
                    Console.WriteLine("Operación cancelada.");
                    return;
                }
                
                _planServices.ActualizarPlan(planActual.Id, planActual.Nombre, planActual.FechaInicio, planActual.FechaFin, planActual.dcto);
                Console.WriteLine("\nPlan actualizado exitosamente!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar el plan: {ex.Message}");
            }
            
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        private string SolicitarTextoOpcional(string mensaje, string valorActual)
        {
            Console.Write(mensaje);
            var nuevoValor = Console.ReadLine()?.Trim();
            
            return string.IsNullOrWhiteSpace(nuevoValor) ? valorActual : nuevoValor;
        }
        
        private DateTime SolicitarFechaOpcional(string mensaje, DateTime valorActual)
        {
            Console.Write(mensaje);
            var nuevoValor = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrWhiteSpace(nuevoValor))
            {
                return valorActual;
            }
            
            if (DateTime.TryParse(nuevoValor, out DateTime resultado))
            {
                return resultado;
            }
            else
            {
                Console.WriteLine("Formato de fecha inválido. Se mantendrá el valor actual.");
                return valorActual;
            }
        }
        
        private double SolicitarNumeroDecimalOpcional(string mensaje, double valorActual)
        {
            Console.Write(mensaje);
            var nuevoValor = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrWhiteSpace(nuevoValor))
            {
                return valorActual;
            }
            
            if (double.TryParse(nuevoValor, out double resultado))
            {
                return resultado;
            }
            else
            {
                Console.WriteLine("Formato de número inválido. Se mantendrá el valor actual.");
                return valorActual;
            }
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
