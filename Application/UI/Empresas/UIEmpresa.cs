using System;
using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Factory;
using SistemaGestorV.Application.UI.Empresas;

namespace SistemaGestorV.Application.UI.Empresas
{
    public class UIEmpresa
    {
        private readonly EmpresaService _empresaServicio;
        private readonly DireccionService _direccionServicio;

        public UIEmpresa(IDbFactory factory)
        {
            _empresaServicio = new EmpresaService(factory.CrearEmpresaRepository());
            _direccionServicio = new DireccionService(factory.CrearDireccionRepository());
        }

        public void GestionEmpresa()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- Gestión de Empresas ---");
                Console.WriteLine("1. Mostrar todas");
                Console.WriteLine("2. Crear nueva");
                Console.WriteLine("3. Actualizar");
                Console.WriteLine("4. Eliminar");
                Console.WriteLine("0. Volver");
                Console.Write("Opción: ");
                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        _empresaServicio.MostrarTodas();
                        break;
                    case "2":
                        var crear = new CrearEmpresa(_empresaServicio, _direccionServicio);
                        crear.Ejecutar();
                        break;
                    case "3":
                        var actualizar = new ActualizarEmpresa(_empresaServicio, _direccionServicio);
                        actualizar.Ejecutar();
                        break;
                    case "4":
                        var eliminar = new EliminarEmpresa(_empresaServicio);
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
