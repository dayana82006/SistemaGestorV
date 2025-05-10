// Actualización para PlanServices.cs

using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlanServices
{
    private readonly IPlanesRepository _repo;
    private readonly ProductoService _productoService;

    public PlanServices(IPlanesRepository repo, ProductoService productoService = null)
    {
        _repo = repo;
        _productoService = productoService ?? new ProductoService();
    }

    public void MostrarTodos()
    {
        try
        {
            var planes = _repo.ObtenerTodos();
            Console.WriteLine("\n--- Lista de Planes ---");
            foreach (var plan in planes)
            {
                Console.WriteLine($"ID: {plan.Id}, Nombre: {plan.Nombre}, Fecha Inicio: {plan.FechaInicio}, Fecha Fin: {plan.FechaFin}, Descuento: {plan.dcto}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError al obtener todos los planes: {ex.Message}");
        }
    }

    public void MostrarProductosDisponibles()
    {
        try
        {
            var productos = _productoService.ObtenerTodos();
            Console.WriteLine("\n--- Productos Disponibles para Planes ---");
            foreach (var producto in productos)
            {
                Console.WriteLine($"ID: {producto.id}, Nombre: {producto.nombre}, Stock: {producto.stock}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError al obtener productos disponibles: {ex.Message}");
        }
    }

    public void CrearPlan(Plan plan)
    {
        try
        {
            _repo.Crear(plan);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError al crear el plan: {ex.Message}");
        }
    }

    public void CrearPlan(Plan plan, List<string> productosAsociados)
    {
        try
        {
            plan.ProductosAsociados = productosAsociados;
            _repo.Crear(plan);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError al crear el plan con productos asociados: {ex.Message}");
        }
    }

    public void ActualizarPlan(int id, string nombre, DateTime fechaInicio, DateTime fechaFin, double dcto)
    {
        try
        {
            var plan = _repo.ObtenerPorId(id);

            if (plan == null)
            {
                Console.WriteLine("Plan no encontrado.");
                return;
            }

            if (string.IsNullOrWhiteSpace(nombre) || fechaInicio == default || fechaFin == default || dcto < 0)
            {
                Console.WriteLine("Datos inválidos para actualizar el plan.");
                return;
            }

            plan.Nombre = nombre.Trim();
            plan.FechaInicio = fechaInicio;
            plan.FechaFin = fechaFin;
            plan.dcto = dcto;

            _repo.Actualizar(plan);
            Console.WriteLine($"Plan ID: {id} actualizado con éxito.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError al actualizar el plan: {ex.Message}");
        }
    }

    public void EliminarPlan(int id)
    {
        try
        {
            var plan = _repo.ObtenerPorId(id);

            if (plan == null)
            {
                Console.WriteLine("Plan no encontrado.");
                return;
            }

            _repo.Eliminar(id);
            Console.WriteLine($"Plan ID: {id} eliminado con éxito.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError al eliminar el plan: {ex.Message}");
        }
    }

    public IEnumerable<Plan> ObtenerPlanes()
    {
        try
        {
            return _repo.ObtenerTodos();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError al obtener todos los planes: {ex.Message}");
            return new List<Plan>(); // Retornar lista vacía en caso de error
        }
    }
    
    public List<SistemaGestorV.Domain.Entities.Producto> ObtenerProductos()
    {
        try
        {
            return _productoService.ObtenerTodos().ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError al obtener productos: {ex.Message}");
            return new List<SistemaGestorV.Domain.Entities.Producto>(); // Retornar lista vacía en caso de error
        }
    }
}