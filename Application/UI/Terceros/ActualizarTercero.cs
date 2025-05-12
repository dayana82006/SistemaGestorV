using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Tercero
{
    public class ActualizarTercero
    {
        private readonly TerceroService _servicio;
        private readonly EpsService _epsService;
        private readonly ArlService _arlService;

        public ActualizarTercero(TerceroService servicio)
        {
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
            
            // Estos servicios deberían ser inyectados, pero para mantener la compatibilidad con el constructor actual,
            // se inicializarán en el método Ejecutar usando el mismo factory que se usa en UITercero
        }

        public void Ejecutar()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("\n--- ACTUALIZAR TERCERO ---");
                
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
                if (tercero.Telefonos != null && tercero.Telefonos.Any())
                {
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
                }
                else
                {
                    Console.WriteLine("No hay teléfonos registrados.");
                    if (Utilidades.LeerConfirmacion("¿Desea agregar un teléfono?"))
                    {
                        if (tercero.Telefonos == null)
                            tercero.Telefonos = new List<Telefono>();
                            
                        string numero = Utilidades.LeerTextoNoVacio("Número: ");
                        string tipo = Utilidades.LeerTextoNoVacio("Tipo: ");
                        tercero.Telefonos.Add(new Telefono { Numero = numero, Tipo = tipo });
                    }
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
                        
                        int epsId = Utilidades.LeerEntero($"Nueva EPS ID (actual: {tercero.Empleado.EpsId}): ");
                        
                        if (epsId <= 0)
                        {
                            Console.WriteLine("ID de EPS inválido. Se mantendrá el valor actual.");
                        }
                        else
                        {
                            tercero.Empleado.EpsId = epsId;
                        }
                        
                        int arlId = Utilidades.LeerEntero($"Nueva ARL ID (actual: {tercero.Empleado.ArlId}): ");
                        
                        if (arlId <= 0)
                        {
                            Console.WriteLine("ID de ARL inválido. Se mantendrá el valor actual.");
                        }
                        else
                        {
                            tercero.Empleado.ArlId = arlId;
                        }
                        break;
                        
                    case 3 when tercero.Proveedor != null: // Proveedor
                        double nuevoDescuento;
                        do
                        {
                            nuevoDescuento = Utilidades.LeerDouble($"\nNuevo descuento (actual: {tercero.Proveedor.Scto}%): ");
                            if (nuevoDescuento < 0 || nuevoDescuento > 100)
                            {
                                Console.WriteLine("El descuento debe estar entre 0 y 100%.");
                            }
                        } while (nuevoDescuento < 0 || nuevoDescuento > 100);
                        
                        tercero.Proveedor.Scto = nuevoDescuento;
                        
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

                // Confirmar la actualización
                if (Utilidades.LeerConfirmacion("\n¿Confirma los cambios realizados?"))
                {
                    _servicio.ActualizarTercero(tercero);
                    Console.WriteLine("\nTercero actualizado con éxito.");
                }
                else
                {
                    Console.WriteLine("\nOperación cancelada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al actualizar tercero: {ex.Message}");
            }
        }
    }
}