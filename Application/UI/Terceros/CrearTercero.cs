using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Tercero
{
    public class CrearTercero
    {
        private readonly TerceroService _servicio;

        public CrearTercero(TerceroService servicio)
        {
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
        }

        public void Ejecutar()
        {
            try
            {
                var tercero = new Domain.Entities.Tercero();
                Console.Clear();
                Console.WriteLine("\n--- CREAR NUEVO TERCERO ---");

                // Datos básicos
                tercero.Nombre = Utilidades.LeerTextoNoVacio("\nNombre: ");
                tercero.Apellidos = Utilidades.LeerTextoNoVacio("Apellidos: ");
                
                string email;
                do
                {
                    email = Utilidades.LeerTextoNoVacio("Email: ");
                    if (!email.Contains("@") || !email.Contains("."))
                    {
                        Console.WriteLine("El email debe tener un formato válido (ejemplo@dominio.com).");
                    }
                } while (!email.Contains("@") || !email.Contains("."));
                tercero.Email = email;

                // Tipo de documento
                Console.WriteLine("\nIMPORTANTE: El ID de Tipo de Documento debe existir en la base de datos.");
                tercero.TipoDocumentoId = Utilidades.LeerEntero("ID Tipo de Documento: ");
                
                // Tipo de Tercero con validación
                int tipoTerceroId;
                do
                {
                    tipoTerceroId = Utilidades.LeerEntero("ID Tipo de Tercero (1: Cliente, 2: Empleado, 3: Proveedor): ");
                    if (tipoTerceroId < 1 || tipoTerceroId > 3)
                    {
                        Console.WriteLine("Tipo de tercero no válido. Debe ser 1, 2 o 3.");
                    }
                } while (tipoTerceroId < 1 || tipoTerceroId > 3);
                tercero.TipoTerceroId = tipoTerceroId;

                Console.WriteLine("\nIMPORTANTE: El ID de Dirección debe existir en la base de datos.");
                tercero.DireccionId = Utilidades.LeerEntero("ID Dirección: ");
                
                Console.WriteLine("\nIMPORTANTE: El ID de Empresa debe existir en la base de datos (EMP001).");
                tercero.EmpresaId = Utilidades.LeerTextoNoVacio("ID Empresa: ");

                switch (tercero.TipoTerceroId)
                {
                    case 1: // Cliente
                        tercero.Cliente = new Cliente();
                        tercero.Cliente.FechaNac = Utilidades.LeerFecha("\nFecha de Nacimiento");
                        tercero.Cliente.FechaInforma = Utilidades.LeerFecha("Fecha de Información");
                        break;
                    
                    case 2: // Empleado
                        tercero.Empleado = new Empleado();
                        tercero.Empleado.FechaIngreso = Utilidades.LeerFecha("\nFecha de Ingreso");
                        tercero.Empleado.SalarioBase = Utilidades.LeerDouble("Salario Base: ");
                        
                        Console.WriteLine("\nIMPORTANTE: Los IDs de EPS y ARL deben existir en la base de datos.");
                        tercero.Empleado.EpsId = Utilidades.LeerEntero("ID EPS: ");
                        tercero.Empleado.ArlId = Utilidades.LeerEntero("ID ARL: ");
                        break;
                    
                    case 3: // Proveedor
                        tercero.Proveedor = new Proveedor();
                        
                        double descuento;
                        do
                        {
                            descuento = Utilidades.LeerDouble("\nDescuento (%): ");
                            if (descuento < 0 || descuento > 100)
                            {
                                Console.WriteLine("El descuento debe ser un porcentaje entre 0 y 100.");
                            }
                        } while (descuento < 0 || descuento > 100);
                        tercero.Proveedor.Scto = descuento;
                        
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

                // Teléfonos
                tercero.Telefonos = new List<Telefono>();
                Console.WriteLine("\n--- INGRESO DE TELÉFONOS ---");
                bool seguirAgregandoTelefonos = true;
                
                while (seguirAgregandoTelefonos)
                {
                    
                    string numero;
                    do
                    {
                        numero = Utilidades.LeerTextoNoVacio("Número (ej. 3101234567): ");
                        if (!numero.All(char.IsDigit))
                        {
                            Console.WriteLine("El número telefónico debe contener solo dígitos.");
                        }
                    } while (!numero.All(char.IsDigit));

                    string tipo = Utilidades.LeerTextoNoVacio("Tipo (ej. Celular, Fijo): ");
                    
                    tercero.Telefonos.Add(new Telefono { Numero = numero, Tipo = tipo });
                    
                    seguirAgregandoTelefonos = Utilidades.LeerConfirmacion("\n¿Desea agregar otro teléfono?");
                }

                // Confirmar la creación del tercero
                if (Utilidades.LeerConfirmacion("\n¿Confirma la creación del tercero?"))
                {
                    _servicio.CrearTercero(tercero);
                    Console.WriteLine("\nTercero creado con éxito.");
                }
                else
                {
                    Console.WriteLine("\nOperación cancelada por el usuario.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Error al crear tercero: {ex.Message}");
                
                if (ex.Message.Contains("foreign key constraint fails"))
                {
                    Console.WriteLine("\nERROR DE REFERENCIA: Uno o más IDs ingresados no existen en la base de datos.");
                    
                    if (ex.Message.Contains("empresaId"))
                        Console.WriteLine("- El ID de Empresa no existe en la base de datos.");
                    
                    if (ex.Message.Contains("tipoDocumentoId"))
                        Console.WriteLine("- El ID de Tipo de Documento no existe en la base de datos.");
                    
                    if (ex.Message.Contains("direccionId"))
                        Console.WriteLine("- El ID de Dirección no existe en la base de datos.");
                    
                    if (ex.Message.Contains("epsId"))
                        Console.WriteLine("- El ID de EPS no existe en la base de datos.");
                    
                    if (ex.Message.Contains("arlId"))
                        Console.WriteLine("- El ID de ARL no existe en la base de datos.");
                    
                    Console.WriteLine("\nVerifique que los IDs existan antes de intentar nuevamente.");
                }
                
                Console.WriteLine("\nPor favor, presione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}