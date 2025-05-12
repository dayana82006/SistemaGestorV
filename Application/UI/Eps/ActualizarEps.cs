using System;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Eps;

public class ActualizarEps
{
    private readonly EpsService _servicio;

    public ActualizarEps(EpsService servicio)
    {
        _servicio = servicio;
    }

public void Ejecutar()
{
    Console.Write("Id Eps a actualizar: ");
    string id = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(id))
    {
        Console.Write("Nuevo Nombre: ");
        string nuevoNombre = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(nuevoNombre))
        {
            bool actualizado = _servicio.ActualizarEps(id, nuevoNombre);

            if (actualizado)
            {
                Console.WriteLine("✅ Eps actualizada con éxito.");
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

