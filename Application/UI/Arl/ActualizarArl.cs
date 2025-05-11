using System;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Arl;

public class ActualizarArl
{
    private readonly ArlService _servicio;

    public ActualizarArl(ArlService servicio)
    {
        _servicio = servicio;
    }

public void Ejecutar()
{
    Console.Write("Id Arl a actualizar: ");
    string id = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(id))
    {
        Console.Write("Nuevo Nombre: ");
        string nuevoNombre = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(nuevoNombre))
        {
            bool actualizado = _servicio.ActualizarArl(id, nuevoNombre);

            if (actualizado)
            {
                Console.WriteLine("✅ Arl actualizada con éxito.");
            }
        }
        else
        {
            Console.WriteLine("❌ Nombre inválido.");
        }
    }
    else
    {
        Console.WriteLine("❌ ID no válido.");
    }
}
}

