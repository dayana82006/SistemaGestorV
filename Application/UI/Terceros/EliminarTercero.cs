using System;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Terceros;

public class EliminarTercero
{
    private readonly TerceroService _servicio;
    public EliminarTercero(TerceroService servicio)
    {
        _servicio = servicio;
    }

    public void Ejecutar()
    {

        Console.Write("ID del Tercero a eliminar: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                _servicio.EliminarTercero(id);

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el Tercero: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("ID inválido.");
        }
    }

}
    