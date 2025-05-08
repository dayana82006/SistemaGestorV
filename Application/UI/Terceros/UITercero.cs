using SistemaGestorV.Application.Services;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;

namespace SistemaGestorV.Application.UI.Tercero
{
    public class UITercero
    {
        private readonly TerceroService _servicio;

        public UITercero(IDbFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _servicio = new TerceroService(factory.CrearTerceroRepository());
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
                Console.WriteLine($"\n--- LISTA DE CLIENTES {terceros.FirstOrDefault()?.TipoTerceroDescripcion.ToUpper()} ---");
                
                foreach (var t in terceros)
                {
                    Console.WriteLine($"ID: {t.Id}, Nombre: {t.Nombre} {t.Apellidos}, Email: {t.Email}");
                }
            }
            else
            {
                Console.WriteLine("Tipo de tercero inválido.");
            }
        }

        private void CrearNuevoTercero()
        {
            Console.Clear();
            Console.WriteLine("\n--- CREAR NUEVO TERCERO ---");
            
            try
            {
                // Seleccionar tipo de tercero
                Console.WriteLine("\nTipos de Tercero disponibles:");
                Console.WriteLine("1. Cliente");
                Console.WriteLine("2. Empleado");
                Console.WriteLine("3. Proveedor");
                
                int tipoTercero = Utilidades.LeerEntero("Seleccione el tipo de tercero (1-3): ");
                if (tipoTercero < 1 || tipoTercero > 3)
                {
                    Console.WriteLine("Tipo de tercero inválido.");
                    return;
                }

                var tercero = new SistemaGestorV.Domain.Entities.Tercero
                {
                    TipoTerceroId = tipoTercero
                };

                // Datos básicos
                tercero.Nombre = Utilidades.LeerTextoNoVacio("\nNombre: ");
                tercero.Apellidos = Utilidades.LeerTextoNoVacio("Apellidos: ");
                tercero.Email = Utilidades.LeerTextoNoVacio("Email: ");
                tercero.TipoDocumentoId = Utilidades.LeerEntero("ID Tipo Documento (ingrese número): ");
                tercero.DireccionId = Utilidades.LeerEntero("ID Dirección (ingrese número): ");
                tercero.EmpresaId = Utilidades.LeerTextoNoVacio("ID Empresa (ingrese texto): ");

                // Validar ID único
                if (_servicio.ExisteTerceroConId(tercero.Id))
                {
                    Console.WriteLine("\nError: Ya existe un tercero con este ID.");
                    return;
                }

                // Teléfonos
                tercero.Telefonos = new List<Telefono>();
                Console.WriteLine("\nIngrese los teléfonos:");

                do
                {
                    string numero = Utilidades.LeerTextoNoVacio("Número (ej. 3101234567): ");
                    string tipo = Utilidades.LeerTextoNoVacio("Tipo (ej. Celular, Fijo): ");

                    tercero.Telefonos.Add(new Telefono { Numero = numero, Tipo = tipo });
                } while (Utilidades.LeerConfirmacion("¿Desea agregar otro teléfono?"));

                // Datos específicos según tipo
                switch (tipoTercero)
                {
                    case 1: // Cliente
                        tercero.Cliente = new Cliente
                        {
                            FechaNac = Utilidades.LeerFecha("\nFecha de Nacimiento"),
                            FechaInforma = Utilidades.LeerFecha("Fecha de Información")
                        };
                        break;
                    
                    case 2: // Empleado
                        tercero.Empleado = new Empleado
                        {
                            FechaIngreso = Utilidades.LeerFecha("\nFecha de Ingreso"),
                            SalarioBase = Utilidades.LeerDouble("Salario Base: "),
                            EpsId = Utilidades.LeerEntero("ID EPS: "),
                            ArlId = Utilidades.LeerEntero("ID ARL: ")
                        };
                        break;
                    
                    case 3: // Proveedor
                        tercero.Proveedor = new Proveedor
                        {
                            Scto = Utilidades.LeerDouble("\nDescuento (%): ")
                        };
                        
                        int diaPago;
                        do
                        {
                            diaPago = Utilidades.LeerEntero("Día de Pago (1-31): ");
                            if (diaPago < 1 || diaPago > 31)
                            {
                                Console.WriteLine("El día de pago debe estar entre 1 y 31.");
                            }
                        } while (diaPago < 1 || diaPago > 31);
                        
                        tercero.Proveedor.DiaPago = diaPago;
                        break;
                }

                _servicio.CrearTercero(tercero);
                Console.WriteLine("\nTercero creado con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear tercero: {ex.Message}");
            }
        }

        private void ActualizarTercero()
        {
            Console.Clear();
            Console.WriteLine("\n--- ACTUALIZAR TERCERO ---");
            
            try
            {
                int id = Utilidades.LeerEntero("ID del tercero a actualizar: ");
                var tercero = _servicio.ObtenerPorId(id);
                
                if (tercero == null)
                {
                    Console.WriteLine("Tercero no encontrado.");
                    return;
                }

                Console.WriteLine($"\nEditando tercero: {tercero.Nombre} {tercero.Apellidos} ({tercero.TipoTerceroDescripcion})");

                // Datos básicos
                tercero.Nombre = Utilidades.LeerTextoNoVacio($"\nNuevo nombre (actual: {tercero.Nombre}): ");
                tercero.Apellidos = Utilidades.LeerTextoNoVacio($"Nuevos apellidos (actual: {tercero.Apellidos}): ");
                tercero.Email = Utilidades.LeerTextoNoVacio($"Nuevo email (actual: {tercero.Email}): ");

                // Teléfonos
                Console.WriteLine("\nTeléfonos actuales:");
                foreach (var tel in tercero.Telefonos)
                {
                    Console.WriteLine($"- {tel.Numero} ({tel.Tipo}) [ID: {tel.Id}]");
                }

                Console.WriteLine("\nOpciones:");
                Console.WriteLine("1. Agregar teléfono");
                Console.WriteLine("2. Eliminar teléfono");
                Console.WriteLine("3. Continuar sin cambios");
                
                int opcionTel = Utilidades.LeerEntero("Seleccione (1-3): ");
                switch (opcionTel)
                {
                    case 1: // Agregar
                        string numero = Utilidades.LeerTextoNoVacio("Número: ");
                        string tipo = Utilidades.LeerTextoNoVacio("Tipo: ");
                        tercero.Telefonos.Add(new Telefono { Numero = numero, Tipo = tipo });
                        break;
                        
                    case 2: // Eliminar
                        int telId = Utilidades.LeerEntero("ID del teléfono a eliminar: ");
                        var tel = tercero.Telefonos.FirstOrDefault(t => t.Id == telId);
                        if (tel != null)
                        {
                            tercero.Telefonos.Remove(tel);
                            Console.WriteLine("Teléfono eliminado.");
                        }
                        else
                        {
                            Console.WriteLine("Teléfono no encontrado.");
                        }
                        break;
                }

                // Datos específicos según tipo
                switch (tercero.TipoTerceroId)
                {
                    case 1 when tercero.Cliente != null: // Cliente
                        tercero.Cliente.FechaNac = Utilidades.LeerFecha($"\nNueva fecha de nacimiento (actual: {tercero.Cliente.FechaNac:yyyy-MM-dd}): ");
                        tercero.Cliente.FechaInforma = Utilidades.LeerFecha($"Nueva fecha de información (actual: {tercero.Cliente.FechaInforma:yyyy-MM-dd}): ");
                        break;
                        
                    case 2 when tercero.Empleado != null: // Empleado
                        tercero.Empleado.FechaIngreso = Utilidades.LeerFecha($"\nNueva fecha de ingreso (actual: {tercero.Empleado.FechaIngreso:yyyy-MM-dd}): ");
                        tercero.Empleado.SalarioBase = Utilidades.LeerDouble($"Nuevo salario base (actual: {tercero.Empleado.SalarioBase:C}): ");
                        break;
                        
                    case 3 when tercero.Proveedor != null: // Proveedor
                        tercero.Proveedor.Scto = Utilidades.LeerDouble($"\nNuevo descuento (actual: {tercero.Proveedor.Scto}%): ");
                        
                        int nuevoDiaPago;
                        do
                        {
                            nuevoDiaPago = Utilidades.LeerEntero($"Nuevo día de pago (actual: {tercero.Proveedor.DiaPago}): ");
                            if (nuevoDiaPago < 1 || nuevoDiaPago > 31)
                            {
                                Console.WriteLine("El día de pago debe estar entre 1 y 31.");
                            }
                        } while (nuevoDiaPago < 1 || nuevoDiaPago > 31);
                        
                        tercero.Proveedor.DiaPago = nuevoDiaPago;
                        break;
                }

                _servicio.ActualizarTercero(tercero);
                Console.WriteLine("\nTercero actualizado con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar tercero: {ex.Message}");
            }
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