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
                
                // Obtener productos disponibles
                var productos = _planServices.ObtenerProductos();
                var productosSeleccionados = new List<string>();
                
                if (productos != null && productos.Count > 0)
                {
                    // Mostrar productos disponibles
                    Console.WriteLine("\nProductos disponibles:");
                    foreach (var producto in productos)
                    {
                        var estaSeleccionado = planActual.ProductosAsociados != null && 
                                              planActual.ProductosAsociados.Any(p => p == producto.nombre);
                        Console.WriteLine($"ID: {producto.id} - Nombre: {producto.nombre} {(estaSeleccionado ? "[Ya seleccionado]" : "")}");
                    }
                    
                    // Solicitar productos uno por uno
                    Console.WriteLine("\nSelección de productos - Se agregarán uno por uno");
                    Console.WriteLine("NOTA: Esto reemplazará todos los productos actualmente asociados al plan.");
                    productosSeleccionados = SolicitarProductosUnoAUno(productos);
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
                
                // Actualizar plan con nuevos productos
                _planServices.ActualizarPlan(planActual.Id, planActual.Nombre, planActual.FechaInicio, planActual.FechaFin, planActual.dcto, productosSeleccionados);
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

        private List<string> SolicitarProductosUnoAUno(List<SistemaGestorV.Domain.Entities.Producto> productos)
        {
            var productosSeleccionados = new List<string>();
            bool continuarAgregando = true;
            
            while (continuarAgregando)
            {
                Console.Write("\nIngrese el ID del producto a agregar: ");
                var idProducto = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrWhiteSpace(idProducto))
                {
                    Console.WriteLine("ID de producto no válido.");
                    continue;
                }
                
                var productoEncontrado = productos.FirstOrDefault(p => p.id != null && p.id.ToString() == idProducto);
                
                if (productoEncontrado == null)
                {
                    Console.WriteLine($"El producto con ID '{idProducto}' no existe. Debe registrar primero el producto con ese ID.");
                    
                    if (!ConfirmarOperacion("¿Desea agregar otro producto? (S/N): "))
                    {
                        continuarAgregando = false;
                    }
                    continue;
                }
                
                if (productosSeleccionados.Contains(productoEncontrado.nombre))
                {
                    Console.WriteLine($"El producto '{productoEncontrado.nombre}' ya está seleccionado.");
                }
                else
                {
                    
                    productosSeleccionados.Add(productoEncontrado.nombre);
                    Console.WriteLine($"Producto '{productoEncontrado.nombre}' agregado correctamente.");
                }
                
                if (!ConfirmarOperacion("¿Desea agregar otro producto? (S/N): "))
                {
                    continuarAgregando = false;
                }
            }
            
            return productosSeleccionados;
        }
    }
}