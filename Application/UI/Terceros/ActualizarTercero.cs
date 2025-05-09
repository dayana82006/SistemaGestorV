using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Tercero
{
    public class ActualizarTercero
    {
        private readonly TerceroService _servicio;

        public ActualizarTercero(TerceroService servicio)
        {
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
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
                        tercero.Empleado.EpsId = Utilidades.LeerEntero($"Nueva EPS ID (actual: {tercero.Empleado.EpsId}): ");
                        tercero.Empleado.ArlId = Utilidades.LeerEntero($"Nueva ARL ID (actual: {tercero.Empleado.ArlId}): ");
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
    }
}