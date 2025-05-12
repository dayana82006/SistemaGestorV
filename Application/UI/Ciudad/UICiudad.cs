using System;
using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Factory;
using SistemaGestorV.Domain.Ports;

namespace SistemaGestorV.Application.UI.Ciudades;

public class UICiudad
{
    private readonly CiudadService _ciudadServicio;
    private readonly RegionService _regionServicio;
    public UICiudad(IDbFactory factory)
    {

        _ciudadServicio = new CiudadService(factory.CrearCiudadRepository());
        _regionServicio = new RegionService(factory.CrearRegionRepository());
    }


    public void GestionCiudad()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n--- Gestion de Ciudades ---");
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
                    _ciudadServicio.MostrarTodos();
                    break;
                case "2":
                    var crear = new CrearCiudad(_ciudadServicio, _regionServicio);
                    crear.Ejecutar();
                    break;
                case "3":
                    var actualizar = new ActualizarCiudad(_regionServicio, _ciudadServicio);
                    actualizar.Ejecutar();
                    break;
                case "4":
                    var eliminar = new EliminarCiudad(_ciudadServicio);
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
