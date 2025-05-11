using System;
using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Factory;
using SistemaGestorV.Domain.Ports;

namespace SistemaGestorV.Application.UI.Eps;

public class UIEps
{
    private readonly EpsService _servicio;
    public UIEps(IDbFactory factory)
    {

        _servicio = new EpsService(factory.CrearEpsRepository());
    }


    public void GestionEpses()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n--- Gestion de Epses ---");
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
                    var crear = new CrearEps(_servicio);
                    crear.Ejecutar();
                    break;
                case "3":
                    var actualizar = new ActualizarEps(_servicio);
                    actualizar.Ejecutar();
                    break;
                case "4":
                    var eliminar = new EliminarEps(_servicio);
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
