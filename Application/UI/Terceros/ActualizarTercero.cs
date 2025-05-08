using SistemaGestorV.Application.Services;

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
        Console.Write("ID del Tercero a actualizar: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Write("Nuevo nombre: ");
            string nuevoNombre = Console.ReadLine();
            
            Console.Write("Correo: ");
             string nuevoCorreo = Console.ReadLine();
            Console.Write("Teléfono: ");
             string nuevoTelefono = Console.ReadLine();
            _servicio.ActualizarTercero(id, nuevoNombre, nuevoCorreo, nuevoTelefono);
            Console.WriteLine("Tercero actualizado.");
        }
        else
        {
            Console.WriteLine("ID inválido.");
        }
    }


}
