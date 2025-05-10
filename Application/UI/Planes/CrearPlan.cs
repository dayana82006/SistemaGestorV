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
                    // Mostrar productos disponibles
                    Console.WriteLine("\nProductos disponibles:");
                    foreach (var producto in productos)
                    {
                        Console.WriteLine($"ID: {producto.id} - Nombre: {producto.nombre}");
                    }
                    
                    // Solicitar productos uno por uno
                    Console.WriteLine("\nSelección de productos - Se agregarán uno por uno");
                    productosSeleccionados = SolicitarProductosUnoAUno(productos);
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
                
                // Buscar producto comparando el id como string para evitar conversiones
                var productoEncontrado = productos.FirstOrDefault(p => p.id != null && p.id.ToString() == idProducto);
                
                if (productoEncontrado == null)
                {
                    Console.WriteLine($"El producto con ID '{idProducto}' no existe. Debe registrar primero el producto con ese ID.");
                    
                    // Preguntar si desea continuar agregando productos
                    if (!ConfirmarOperacion("¿Desea agregar otro producto? (S/N): "))
                    {
                        continuarAgregando = false;
                    }
                    continue;
                }
                
                // Verificar si el producto ya está seleccionado
                if (productosSeleccionados.Contains(productoEncontrado.nombre))
                {
                    Console.WriteLine($"El producto '{productoEncontrado.nombre}' ya está seleccionado.");
                }
                else
                {
                    // Agregar el nombre del producto, no el ID
                    productosSeleccionados.Add(productoEncontrado.nombre);
                    Console.WriteLine($"Producto '{productoEncontrado.nombre}' agregado correctamente.");
                }
                
                // Preguntar si desea continuar agregando productos
                if (!ConfirmarOperacion("¿Desea agregar otro producto? (S/N): "))
                {
                    continuarAgregando = false;
                }
            }
            
            return productosSeleccionados;
        }
    }
}