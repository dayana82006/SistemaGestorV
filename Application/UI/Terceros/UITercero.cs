using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Factory;
using SistemaGestorV.Domain.Ports;

namespace SistemaGestorV.Application.UI.Tercero
{
    public class UITercero
    {
        private readonly TerceroService _servicio;
        private readonly EmpresaService _empresaService;
        private readonly TipoDocumentoService _tipoDocumentoService;
        private readonly DireccionService _direccionService;
        private readonly EpsService _epsService;
        private readonly ArlService _arlService;

        public IDbFactory Factory { get; }

        public UITercero(IDbFactory factory)
        {
            Factory = factory;
            _servicio = new TerceroService(factory.CrearTerceroRepository());
            _empresaService = new EmpresaService(factory.CrearEmpresaRepository());
            _tipoDocumentoService = new TipoDocumentoService(factory.CrearTipoDocumentoRepository());
            _direccionService = new DireccionService(factory.CrearDireccionRepository());
            _epsService = new EpsService(factory.CrearEpsRepository());
            _arlService = new ArlService(factory.CrearArlRepository());
        }

        public void GestionarTerceros()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- MENÚ TERCEROS ---");
                Console.WriteLine("1. Mostrar todos los terceros");
                Console.WriteLine("2. Mostrar por tipo");
                Console.WriteLine("3. Crear nuevo tercero");
                Console.WriteLine("4. Actualizar tercero");
                Console.WriteLine("5. Eliminar tercero");
                Console.WriteLine("0. Volver");
                Console.Write("Opción: ");
                
                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MostrarTodos();
                        break;
                    case "2":
                        MostrarPorTipo();
                        break;
                    case "3":
                        CrearNuevoTercero();
                        break;
                    case "4":
                        ActualizarTercero();
                        break;
                    case "5":
                        EliminarTercero();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

                Console.WriteLine("\nPresiona una tecla para continuar...");
                Console.ReadKey();
            }
        }

        private void MostrarTodos()
        {
            Console.Clear();
            _servicio.MostrarTodos();
        }

        private void MostrarPorTipo()
        {
            Console.Clear();
            Console.WriteLine("\n--- MOSTRAR TERCEROS POR TIPO ---");
            Console.WriteLine("1. Clientes");
            Console.WriteLine("2. Empleados");
            Console.WriteLine("3. Proveedores");
            
            int tipo = Utilidades.LeerEntero("Seleccione el tipo (1-3): ");
            if (tipo >= 1 && tipo <= 3)
            {
                var terceros = _servicio.ObtenerPorTipo(tipo);
                var tipoDescripcion = tipo == 1 ? "CLIENTES" : (tipo == 2 ? "EMPLEADOS" : "PROVEEDORES");
                Console.WriteLine($"\n--- LISTA DE {tipoDescripcion} ---");
                
                if (!terceros.Any())
                {
                    Console.WriteLine($"No hay {tipoDescripcion.ToLower()} registrados.");
                    return;
                }
                
                foreach (var t in terceros)
                {
                    Console.WriteLine($"ID: {t.Id}, Nombre: {t.Nombre} {t.Apellidos}, Email: {t.Email}");
                    
                    if (t.Telefonos?.Count > 0)
                    {
                        Console.WriteLine("   Teléfonos:");
                        foreach (var tel in t.Telefonos)
                        {
                            Console.WriteLine($"    - {tel.Numero} ({tel.Tipo})");
                        }
                    }
                    
                    switch (t.TipoTerceroId)
                    {
                        case 1 when t.Cliente != null:
                            Console.WriteLine($"   F.Nac: {t.Cliente.FechaNac:yyyy-MM-dd}, F.Informa: {t.Cliente.FechaInforma:yyyy-MM-dd}");
                            break;
                        case 2 when t.Empleado != null:
                            Console.WriteLine($"   Salario: {t.Empleado.SalarioBase:C}, Ingreso: {t.Empleado.FechaIngreso:yyyy-MM-dd}");
                            break;
                        case 3 when t.Proveedor != null:
                            Console.WriteLine($"   Dcto: {t.Proveedor.Scto}%, Día Pago: {t.Proveedor.DiaPago}");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Tipo de tercero inválido.");
            }
        }

        private void CrearNuevoTercero()
        {
            var creador = new CrearTercero(
                _servicio, 
                _empresaService, 
                _tipoDocumentoService, 
                _direccionService, 
                _epsService, 
                _arlService);
                
            creador.Ejecutar();
        }

        private void ActualizarTercero()
        {
            var actualizador = new ActualizarTercero(_servicio);
            actualizador.Ejecutar();
        }

        private void EliminarTercero()
        {
            Console.Clear();
            Console.WriteLine("\n--- ELIMINAR TERCERO ---");
            
            try
            {
                int id = Utilidades.LeerEntero("ID del tercero a eliminar: ");
                var tercero = _servicio.ObtenerPorId(id);
                
                if (tercero == null)
                {
                    Console.WriteLine("Tercero no encontrado.");
                    return;
                }

                bool confirmar = Utilidades.LeerConfirmacion($"\nEstá seguro que desea eliminar a {tercero.Nombre} {tercero.Apellidos}?");
                if (confirmar)
                {
                    _servicio.EliminarTercero(id);
                    Console.WriteLine("\nTercero eliminado con éxito.");
                }
                else
                {
                    Console.WriteLine("\nOperación cancelada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al eliminar tercero: {ex.Message}");
            }
        }
    }
}