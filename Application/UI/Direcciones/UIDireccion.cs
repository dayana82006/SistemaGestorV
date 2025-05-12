using System;
using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Factory;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Application.UI.Direcciones;

namespace SistemaGestorV.Application.UI.Direcciones
{
    public class UIDireccion
    {
        private readonly DireccionService _direccionServicio;
        private readonly CiudadService _ciudadServicio;

        public UIDireccion(IDbFactory factory)
        {
            _direccionServicio = new DireccionService(factory.CrearDireccionRepository());
            _ciudadServicio = new CiudadService(factory.CrearCiudadRepository());
        }

        public void GestionDireccion()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- Gestión de Direcciones ---");
                Console.WriteLine("1. Mostrar todos");
                Console.WriteLine("2. Crear nueva");
                Console.WriteLine("3. Actualizar");
                Console.WriteLine("4. Eliminar");
                Console.WriteLine("0. Volver");
                Console.Write("Opción: ");
                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        _direccionServicio.MostrarTodos();
                        break;
                    case "2":
                        var crear = new CrearDireccion(_direccionServicio, _ciudadServicio);
                        crear.Ejecutar();
                        break;
                    case "3":
                        var actualizar = new ActualizarDireccion(_direccionServicio, _ciudadServicio);
                        actualizar.Ejecutar();
                        break;
                    case "4":
                        var eliminar = new EliminarDireccion(_direccionServicio);
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
