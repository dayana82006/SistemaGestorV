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
                        var creador = new CrearTercero(_servicio);
                        creador.Ejecutar();
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
            Console.WriteLine("\n--- LISTA DE TODOS LOS TERCEROS ---");
            _servicio.MostrarTodos();
        }

        private void MostrarPorTipo()
        {
            Console.Clear();
            Console.WriteLine("\n--- MOSTRAR TERCEROS POR TIPO ---");
            Console.WriteLine("1. Clientes");
            Console.WriteLine("2. Empleados");
            Console.WriteLine("3. Proveedores");
            Console.Write("Seleccione el tipo: ");
            
            if (int.TryParse(Console.ReadLine(), out int tipo) && tipo >= 1 && tipo <= 3)
            {
                var terceros = _servicio.ObtenerPorTipo(tipo);
                Console.WriteLine($"\n--- LISTA DE {terceros.FirstOrDefault()?.TipoTerceroDescripcion.ToUpper()}S ---");
                
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
                Console.Write("Seleccione el tipo de tercero: ");
                
                if (!int.TryParse(Console.ReadLine(), out int tipoTercero) || tipoTercero < 1 || tipoTercero > 3)
                {
                    Console.WriteLine("Tipo de tercero inválido.");
                    return;
                }

                var tercero = new SistemaGestorV.Domain.Entities.Tercero
                {
                    TipoTerceroId = tipoTercero
                };


                // Datos básicos
                Console.Write("\nNombre: ");
                tercero.Nombre = Console.ReadLine();

                Console.Write("Apellidos: ");
                tercero.Apellidos = Console.ReadLine();

                Console.Write("Email: ");
                tercero.Email = Console.ReadLine();

                Console.Write("ID Tipo Documento (ingrese número): ");
                if (int.TryParse(Console.ReadLine(), out int tipoDoc))
                    tercero.TipoDocumentoId = tipoDoc;

                Console.Write("ID Dirección (ingrese número): ");
                if (int.TryParse(Console.ReadLine(), out int dirId))
                    tercero.DireccionId = dirId;

                Console.Write("ID Empresa (ingrese texto): ");
                tercero.EmpresaId = Console.ReadLine();

                // Teléfonos
                tercero.Telefonos = new List<Telefono>();
                Console.WriteLine("\nIngrese los teléfonos (deje vacío para terminar):");
                while (true)
                {
                    Console.Write("Número (ej. 3101234567): ");
                    var numero = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(numero)) break;

                    Console.Write("Tipo (ej. Celular, Fijo): ");
                    var tipo = Console.ReadLine();

                    tercero.Telefonos.Add(new Telefono { Numero = numero, Tipo = tipo });
                }

                // Datos específicos según tipo
                switch (tipoTercero)
                {
                    case 1: // Cliente
                        tercero.Cliente = new Cliente();
                        Console.Write("\nFecha de Nacimiento (yyyy-mm-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime fechaNac))
                            tercero.Cliente.FechaNac = fechaNac;

                        Console.Write("Fecha de Información (yyyy-mm-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime fechaInfo))
                            tercero.Cliente.FechaInforma = fechaInfo;
                        break;
                    
                    case 2: // Empleado
                        tercero.Empleado = new Empleado();
                        Console.Write("\nFecha de Ingreso (yyyy-mm-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime fechaIngreso))
                            tercero.Empleado.FechaIngreso = fechaIngreso;

                        Console.Write("Salario Base: ");
                        if (double.TryParse(Console.ReadLine(), out double salario))
                            tercero.Empleado.SalarioBase = salario;

                        Console.Write("ID EPS: ");
                        if (int.TryParse(Console.ReadLine(), out int epsId))
                            tercero.Empleado.EpsId = epsId;

                        Console.Write("ID ARL: ");
                        if (int.TryParse(Console.ReadLine(), out int arlId))
                            tercero.Empleado.ArlId = arlId;
                        break;
                    
                    case 3: // Proveedor
                        tercero.Proveedor = new Proveedor();
                        Console.Write("\nDescuento (%): ");
                        if (double.TryParse(Console.ReadLine(), out double descuento))
                            tercero.Proveedor.Scto = descuento;

                        Console.Write("Día de Pago (1-31): ");
                        if (int.TryParse(Console.ReadLine(), out int diaPago) && diaPago >= 1 && diaPago <= 31)
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
                Console.Write("ID del tercero a actualizar: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("ID inválido.");
                    return;
                }

                var tercero = _servicio.ObtenerPorId(id);
                if (tercero == null)
                {
                    Console.WriteLine("Tercero no encontrado.");
                    return;
                }

                Console.WriteLine($"\nEditando tercero: {tercero.Nombre} {tercero.Apellidos} ({tercero.TipoTerceroDescripcion})");

                // Datos básicos
                Console.Write("\nNuevo nombre (actual: {0}): ", tercero.Nombre);
                var nuevoNombre = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nuevoNombre))
                    tercero.Nombre = nuevoNombre;

                Console.Write("Nuevos apellidos (actual: {0}): ", tercero.Apellidos);
                var nuevosApellidos = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nuevosApellidos))
                    tercero.Apellidos = nuevosApellidos;

                Console.Write("Nuevo email (actual: {0}): ", tercero.Email);
                var nuevoEmail = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nuevoEmail))
                    tercero.Email = nuevoEmail;

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
                Console.Write("Seleccione: ");
                
                if (int.TryParse(Console.ReadLine(), out int opcionTel))
                {
                    switch (opcionTel)
                    {
                        case 1: // Agregar
                            Console.Write("Número: ");
                            var numero = Console.ReadLine();
                            Console.Write("Tipo: ");
                            var tipo = Console.ReadLine();
                            
                            if (!string.IsNullOrWhiteSpace(numero) && !string.IsNullOrWhiteSpace(tipo))
                            {
                                tercero.Telefonos.Add(new Telefono { Numero = numero, Tipo = tipo });
                            }
                            break;
                            
                        case 2: // Eliminar
                            Console.Write("ID del teléfono a eliminar: ");
                            if (int.TryParse(Console.ReadLine(), out int telId))
                            {
                                var tel = tercero.Telefonos.FirstOrDefault(t => t.Id == telId);
                                if (tel != null)
                                {
                                    tercero.Telefonos.Remove(tel);
                                }
                            }
                            break;
                    }
                }

                // Datos específicos según tipo
                switch (tercero.TipoTerceroId)
                {
                    case 1 when tercero.Cliente != null: // Cliente
                        Console.Write("\nNueva fecha de nacimiento (actual: {0:yyyy-MM-dd}): ", tercero.Cliente.FechaNac);
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime nuevaFechaNac))
                            tercero.Cliente.FechaNac = nuevaFechaNac;

                        Console.Write("Nueva fecha de información (actual: {0:yyyy-MM-dd}): ", tercero.Cliente.FechaInforma);
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime nuevaFechaInfo))
                            tercero.Cliente.FechaInforma = nuevaFechaInfo;
                        break;
                        
                    case 2 when tercero.Empleado != null: // Empleado
                        Console.Write("\nNueva fecha de ingreso (actual: {0:yyyy-MM-dd}): ", tercero.Empleado.FechaIngreso);
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime nuevaFechaIngreso))
                            tercero.Empleado.FechaIngreso = nuevaFechaIngreso;

                        Console.Write("Nuevo salario base (actual: {0:C}): ", tercero.Empleado.SalarioBase);
                        if (double.TryParse(Console.ReadLine(), out double nuevoSalario))
                            tercero.Empleado.SalarioBase = nuevoSalario;
                        break;
                        
                    case 3 when tercero.Proveedor != null: // Proveedor
                        Console.Write("\nNuevo descuento (actual: {0}%): ", tercero.Proveedor.Scto);
                        if (double.TryParse(Console.ReadLine(), out double nuevoDescuento))
                            tercero.Proveedor.Scto = nuevoDescuento;

                        Console.Write("Nuevo día de pago (actual: {0}): ", tercero.Proveedor.DiaPago);
                        if (int.TryParse(Console.ReadLine(), out int nuevoDiaPago) && nuevoDiaPago >= 1 && nuevoDiaPago <= 31)
                            tercero.Proveedor.DiaPago = nuevoDiaPago;
                        break;
                }

                _servicio.ActualizarTercero(tercero);
                Console.WriteLine("\n✅ Tercero actualizado con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error al actualizar tercero: {ex.Message}");
            }
        }

        private void EliminarTercero()
        {
            Console.Clear();
            Console.WriteLine("\n--- ELIMINAR TERCERO ---");
            
            try
            {
                Console.Write("ID del tercero a eliminar: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("ID inválido.");
                    return;
                }

                var tercero = _servicio.ObtenerPorId(id);
                if (tercero == null)
                {
                    Console.WriteLine("Tercero no encontrado.");
                    return;
                }

                Console.WriteLine($"\nEstá seguro que desea eliminar a {tercero.Nombre} {tercero.Apellidos}? (S/N)");
                var confirmacion = Console.ReadLine()?.ToUpper();

                if (confirmacion == "S")
                {
                    _servicio.EliminarTercero(id);
                    Console.WriteLine("\n✅ Tercero eliminado con éxito.");
                }
                else
                {
                    Console.WriteLine("\nOperación cancelada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error al eliminar tercero: {ex.Message}");
            }
        }
    }
}