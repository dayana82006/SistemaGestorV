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
        var planes = _repo.ObtenerTodos();
        Console.WriteLine("\n--- Lista de Planes ---");
        foreach (var plan in planes)
        {
            Console.WriteLine($"ID: {plan.Id}, Nombre: {plan.Nombre}, Fecha Inicio: {plan.FechaInicio}, Fecha Fin: {plan.FechaFin}, Descuento: {plan.Descuento}");
        }
    }

    public void MostrarProductosDisponibles()
    {
        var productos = _productoService.ObtenerTodos();
        Console.WriteLine("\n--- Productos Disponibles para Planes ---");
        foreach (var producto in productos)
        {
            Console.WriteLine($"ID: {producto.id}, Nombre: {producto.nombre}, Stock: {producto.stock}");
        }
    }

    public void CrearPlan(Plan plan)
    {
        _repo.Crear(plan);
    }

    public void CrearPlan(Plan plan, List<string> productosAsociados)
    {
        plan.ProductosAsociados = productosAsociados;
        _repo.Crear(plan);
    }

    public void ActualizarPlan(int id, string nombre, DateTime fechaInicio, DateTime fechaFin, double descuento)
    {
        var plan = _repo.ObtenerPorId(id.ToString());

        if (plan == null)
        {
            Console.WriteLine("Plan no encontrado.");
            return;
        }

        if (string.IsNullOrWhiteSpace(nombre) || fechaInicio == default || fechaFin == default || descuento < 0)
        {
            Console.WriteLine("Datos inválidos para actualizar el plan.");
            return;
        }

        plan.Nombre = nombre.Trim();
        plan.FechaInicio = fechaInicio;
        plan.FechaFin = fechaFin;
        plan.Descuento = descuento;

        _repo.Actualizar(plan);
        Console.WriteLine($"Plan ID: {id} actualizado con éxito.");
    }

    public void EliminarPlan(string id)
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

    public IEnumerable<Plan> ObtenerPlanes()
    {
        return _repo.ObtenerTodos();
    }
    
    public List<Producto> ObtenerProductos()
    {
        return _productoService.ObtenerTodos().ToList();
    }
}