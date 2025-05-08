using SistemaGestorV.Application.Services;
using SistemaGestorV;
namespace SistemaGestorV.Application.UI.Terceros;

public class ActualizarTercero
{
    private readonly TerceroService _servicio;
    public ActualizarTercero(TerceroService servicio)
    {
        _servicio = servicio;
    }
    public void Ejecutar()
    {
        Console.Clear();
        Console.Write("ID del Tercero a actualizar: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            string nuevoNombre = Utilidades.LeerTextoNoVacio("Nuevo nombre:");
            string nuevoCorreo = Utilidades.LeerTextoNoVacio("Correo:");
            string nuevoTelefono = Utilidades.LeerTextoNoVacio("Teléfono:");

            _servicio.ActualizarTercero(id, nuevoNombre, nuevoCorreo, nuevoTelefono);
            Console.WriteLine("Tercero actualizado.");
        }
        else
        {
            Console.WriteLine("ID inválido.");
        }
    }
}