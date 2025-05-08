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
                Console.Write("\nNombre: ");
                tercero.Nombre = Console.ReadLine();

                Console.Write("Apellidos: ");
                tercero.Apellidos = Console.ReadLine();

                Console.Write("Email: ");
                tercero.Email = Console.ReadLine();

                Console.Write("ID Tipo de Documento: ");
                if (int.TryParse(Console.ReadLine(), out int tipoDocId))
                    tercero.TipoDocumentoId = tipoDocId;

                Console.Write("ID Tipo de Tercero (1: Cliente, 2: Empleado, 3: Proveedor): ");
                if (!int.TryParse(Console.ReadLine(), out int tipoTerceroId) || tipoTerceroId < 1 || tipoTerceroId > 3)
                {
                    Console.WriteLine("Tipo de tercero no válido.");
                    return;
                }
                tercero.TipoTerceroId = tipoTerceroId;

                Console.Write("ID Dirección: ");
                if (int.TryParse(Console.ReadLine(), out int direccionId))
                    tercero.DireccionId = direccionId;

                Console.Write("ID Empresa: ");
                tercero.EmpresaId = Console.ReadLine();

                // Datos específicos según el tipo de tercero
                switch (tercero.TipoTerceroId)
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

                _servicio.CrearTercero(tercero);
                Console.WriteLine("\n✅ Tercero creado con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear tercero: {ex.Message}");
            }
        }
    }
}