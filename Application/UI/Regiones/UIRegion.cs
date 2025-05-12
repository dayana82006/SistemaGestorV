using System;
using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Factory;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.UI.Regioneses;

namespace SistemaGestorV.Application.UI.Regiones
{
    public class UIRegion
    {
        private readonly RegionService _regionServicio;
        private readonly PaisService _paisServicio;

        public UIRegion(IDbFactory factory)
        {
            _regionServicio = new RegionService(factory.CrearRegionRepository());
            _paisServicio = new PaisService(factory.CrearPaisRepository());
        }

        public void GestionRegion()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- Gestión de Regiones ---");
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
                        _regionServicio.MostrarTodos();
                        break;
                    case "2":
                        var crear = new CrearRegion(_regionServicio, _paisServicio);
                        crear.Ejecutar();
                        break;
                    case "3":
                        var actualizar = new ActualizarRegion(_regionServicio, _paisServicio);
                        actualizar.Ejecutar();
                        break;
                    case "4":
                        var eliminar = new EliminarRegion(_regionServicio);
                        eliminar.Ejecutar();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("❌ Opción no válida.");
                        break;
                }

                Console.WriteLine("\nPresiona una tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}
