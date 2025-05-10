using System;
using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Factory;
using SistemaGestorV.Domain.Ports;

namespace SistemaGestorV.Application.UI.Producto;

public class UIProducto
{
    private readonly ProductoService _servicio;
    public UIProducto(IDbFactory factory)
    {

        _servicio = new ProductoService(factory.CrearProductoRepository());
    }


    public void MostrarMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n--- Gestion de Productos ---");
            Console.WriteLine("1. Mostrar todos");
            Console.WriteLine("2. Crear nuevo");
            Console.WriteLine("3. Actualizar");
            Console.WriteLine("4. Eliminar");
            Console.WriteLine("0. Volver");
            Console.Write("Opción: ");
            var opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    _servicio.MostrarTodos();
                    break;
                case "2":
                    var crear = new CrearProducto(_servicio);
                    crear.Ejecutar();
                    break;
                case "3":
                    var actualizar = new ActualizarProducto(_servicio);
                    actualizar.Ejecutar();
                    break;
                case "4":
                    var eliminar = new EliminarProducto(_servicio);
                    eliminar.Ejecutar();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Opción no valida.");
                    break;
            }

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();
        }
    }
}
