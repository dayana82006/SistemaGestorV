using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Factory;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Infrastructure.Repositories;
using SistemaGestorV.Infrastructure.Mysql;
using SistemaGestorV.Application.UI.Compra;

namespace SistemaGestorV.Application.UI.Compras;
public class UICompra
{
    private readonly CompraService _servicio;

    public UICompra(IDbFactory factory)
    {
        var repoCompra = factory.CrearCompraRepository();
        var repoDetalle = factory.CrearDetalleCompraRepository();
        _servicio = new CompraService(repoCompra, repoDetalle);
    }

    public void MostrarMenu()
    {
        Console.Clear();
        Console.WriteLine("🛒 Menu de Compras");
        Console.WriteLine("1. Mostrar todas las compras");
        Console.WriteLine("2. Agregar nueva compra");
        Console.WriteLine("3. Actualizar compra");
        Console.WriteLine("4. Eliminar compra");
        Console.WriteLine("0. Salir");
        Console.Write("Seleccione una opción: ");
        var opcion = Console.ReadLine();

        switch (opcion)
        {
            case "1":
                _servicio.MostrarTodo();
                break;
            case "2":
                var crear = new CrearCompra(_servicio);
                crear.Ejecutar();
                break;
            case "3":
               var actualizar = new ActualizarCompra(_servicio);
               actualizar.Ejecutar();
                break;
            case "4":
                var eliminar = new EliminarCompra(_servicio);
                eliminar.Ejecutar();
                break;  
            case "0":
                return;
            default:
                Console.WriteLine("❌ Opción no válida.");
                break;
        }
    }
}
