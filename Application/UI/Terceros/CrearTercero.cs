using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Tercero
{
    public class CrearTercero
    {
        private readonly TerceroService _servicio;
        private readonly EmpresaService _empresaService;
        private readonly TipoDocumentoService _tipoDocumentoService;
        private readonly DireccionService _direccionService;
        private readonly EpsService _epsService;
        private readonly ArlService _arlService;

        public CrearTercero(
            TerceroService servicio,
            EmpresaService empresaService,
            TipoDocumentoService tipoDocumentoService,
            DireccionService direccionService,
            EpsService epsService,
            ArlService arlService)
        {
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
            _empresaService = empresaService ?? throw new ArgumentNullException(nameof(empresaService));
            _tipoDocumentoService = tipoDocumentoService ?? throw new ArgumentNullException(nameof(tipoDocumentoService));
            _direccionService = direccionService ?? throw new ArgumentNullException(nameof(direccionService));
            _epsService = epsService ?? throw new ArgumentNullException(nameof(epsService));
            _arlService = arlService ?? throw new ArgumentNullException(nameof(arlService));
        }

        public void Ejecutar()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("\n--- CREAR NUEVO TERCERO ---");

                Console.Write("\nID Empresa (puede ser numérico o alfanumérico): ");
                string empresaId = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(empresaId))
                {
                    Console.WriteLine("El ID de empresa no puede estar vacío.");
                    return;
                }

                var empresaExistente = _empresaService.ObtenerPorId(empresaId);
                if (empresaExistente == null)
                {
                    Console.WriteLine($"El ID de Empresa '{empresaId}' no existe. Primero regístrelo.");
                    return;
                }

                Console.Write("ID Tipo de Documento: ");
                if (!int.TryParse(Console.ReadLine(), out int tipoDocumentoId))
                {
                    Console.WriteLine("El ID de Tipo de Documento debe ser un número entero.");
                    return;
                }

                var tipoDocumentoExistente = _tipoDocumentoService.ObtenerPorId(tipoDocumentoId.ToString());
                if (tipoDocumentoExistente == null)
                {
                    Console.WriteLine($"El ID de Tipo de Documento '{tipoDocumentoId}' no existe. Primero regístrelo.");
                    return;
                }

                Console.Write("ID Dirección: ");
                if (!int.TryParse(Console.ReadLine(), out int direccionId))
                {
                    Console.WriteLine("El ID de Dirección debe ser un número entero.");
                    return;
                }

                var direccionExistente = _direccionService.ObtenerPorId(direccionId);
                if (direccionExistente == null)
                {
                    Console.WriteLine($"El ID de Dirección '{direccionId}' no existe. Primero regístrelo.");
                    return;
                }

                int tipoTerceroId;
                do
                {
                    Console.Write("ID Tipo de Tercero (1: Cliente, 2: Empleado, 3: Proveedor): ");
                    if (!int.TryParse(Console.ReadLine(), out tipoTerceroId))
                    {
                        Console.WriteLine("Debe ingresar un número válido.");
                        continue;
                    }

                    if (tipoTerceroId < 1 || tipoTerceroId > 3)
                    {
                        Console.WriteLine("Tipo de tercero no válido. Debe ser 1, 2 o 3.");
                    }
                } while (tipoTerceroId < 1 || tipoTerceroId > 3);

                int? epsId = null;
                int? arlId = null;

                if (tipoTerceroId == 2) // Empleado
                {
                    // Validar EPS
                    Console.Write("ID EPS: ");
                    if (!int.TryParse(Console.ReadLine(), out int tempEpsId))
                    {
                        Console.WriteLine("El ID de EPS debe ser un número entero.");
                        return;
                    }

                    var epsExistente = _epsService.ObtenerPorId(tempEpsId.ToString());
                    if (epsExistente == null)
                    {
                        Console.WriteLine($"El ID de EPS '{tempEpsId}' no existe. Primero regístrelo.");
                        return;
                    }
                    epsId = tempEpsId;

                    Console.Write("ID ARL: ");
                    if (!int.TryParse(Console.ReadLine(), out int tempArlId))
                    {
                        Console.WriteLine("El ID de ARL debe ser un número entero.");
                        return;
                    }

                    var arlExistente = _arlService.ObtenerPorId(tempArlId.ToString());
                    if (arlExistente == null)
                    {
                        Console.WriteLine($"El ID de ARL '{tempArlId}' no existe. Primero regístrelo.");
                        return;
                    }
                    arlId = tempArlId;
                }

                Console.Write("\nNombre: ");
                string nombre = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    Console.WriteLine("El nombre no puede estar vacío.");
                    return;
                }

                Console.Write("Apellidos: ");
                string apellidos = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(apellidos))
                {
                    Console.WriteLine("Los apellidos no pueden estar vacíos.");
                    return;
                }

                string email;
                do
                {
                    Console.Write("Email: ");
                    email = Console.ReadLine()?.Trim();
                    if (!email.Contains("@") || !email.Contains("."))
                    {
                        Console.WriteLine("El email debe tener un formato válido (ejemplo@dominio.com).");
                    }
                } while (!email.Contains("@") || !email.Contains("."));

                var tercero = new Domain.Entities.Tercero
                {
                    Nombre = nombre,
                    Apellidos = apellidos,
                    Email = email,
                    TipoDocumentoId = tipoDocumentoId,
                    TipoTerceroId = tipoTerceroId,
                    DireccionId = direccionId,
                    EmpresaId = empresaId
                };

                switch (tercero.TipoTerceroId)
                {
                    case 1: // Cliente
                        Console.Write("\nFecha de Nacimiento (YYYY-MM-DD): ");
                        if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaNac))
                        {
                            Console.WriteLine("Formato de fecha inválido.");
                            return;
                        }

                        Console.Write("Fecha de Información (YYYY-MM-DD): ");
                        if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaInforma))
                        {
                            Console.WriteLine("Formato de fecha inválido.");
                            return;
                        }

                        tercero.Cliente = new Cliente
                        {
                            FechaNac = fechaNac,
                            FechaInforma = fechaInforma
                        };
                        break;
                    
                    case 2: // Empleado
                        Console.Write("\nFecha de Ingreso (YYYY-MM-DD): ");
                        if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaIngreso))
                        {
                            Console.WriteLine("Formato de fecha inválido.");
                            return;
                        }

                        Console.Write("Salario Base: ");
                        if (!double.TryParse(Console.ReadLine(), out double salarioBase))
                        {
                            Console.WriteLine("El salario debe ser un número válido.");
                            return;
                        }

                        tercero.Empleado = new Empleado
                        {
                            FechaIngreso = fechaIngreso,
                            SalarioBase = salarioBase,
                            EpsId = epsId.Value,
                            ArlId = arlId.Value
                        };
                        break;
                    
                    case 3: // Proveedor
                        double descuento;
                        do
                        {
                            Console.Write("\nDescuento (%): ");
                            if (!double.TryParse(Console.ReadLine(), out descuento))
                            {
                                Console.WriteLine("El descuento debe ser un número válido.");
                                continue;
                            }

                            if (descuento < 0 || descuento > 100)
                            {
                                Console.WriteLine("El descuento debe ser un porcentaje entre 0 y 100.");
                            }
                        } while (descuento < 0 || descuento > 100);
                        
                        int diaPago;
                        do
                        {
                            Console.Write("Día de Pago (1-31): ");
                            if (!int.TryParse(Console.ReadLine(), out diaPago))
                            {
                                Console.WriteLine("El día de pago debe ser un número entero.");
                                continue;
                            }

                            if (diaPago < 1 || diaPago > 31)
                            {
                                Console.WriteLine("El día de pago debe estar entre 1 y 31.");
                            }
                        } while (diaPago < 1 || diaPago > 31);
                        
                        tercero.Proveedor = new Proveedor
                        {
                            Scto = descuento,
                            DiaPago = diaPago
                        };
                        break;
                }

                // Teléfonos
                tercero.Telefonos = new List<Telefono>();
                Console.WriteLine("\n--- INGRESO DE TELÉFONOS ---");
                bool agregarMasTelefonos = true;
                
                while (agregarMasTelefonos)
                {
                    Console.Write("Número de teléfono: ");
                    string numero = Console.ReadLine()?.Trim();
                    if (string.IsNullOrWhiteSpace(numero))
                    {
                        Console.WriteLine("El número no puede estar vacío.");
                        continue;
                    }

                    Console.Write("Tipo (ej. Celular, Fijo): ");
                    string tipo = Console.ReadLine()?.Trim();
                    if (string.IsNullOrWhiteSpace(tipo))
                    {
                        Console.WriteLine("El tipo no puede estar vacío.");
                        continue;
                    }

                    tercero.Telefonos.Add(new Telefono { Numero = numero, Tipo = tipo });
                    
                    Console.Write("\n¿Desea agregar otro teléfono? (S/N): ");
                    agregarMasTelefonos = Console.ReadLine()?.Trim().ToUpper() == "S";
                }

                Console.Write("\n¿Confirma la creación del tercero? (S/N): ");
                if (Console.ReadLine()?.Trim().ToUpper() == "S")
                {
                    _servicio.CrearTercero(tercero);
                    Console.WriteLine("\n✅ Tercero creado con éxito.");
                }
                else
                {
                    Console.WriteLine("\nOperación cancelada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error al crear tercero: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}