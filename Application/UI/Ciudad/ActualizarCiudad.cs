using System;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Ciudad;

public class ActualizarCiudad
{
    private readonly CiudadService _servicio;

    public ActualizarCiudad(CiudadService servicio)
    {
        _servicio = servicio;
    }

public void Ejecutar()
{
    Console.Write("Id Ciudad a actualizar: ");
    string id = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(id))
    {
        Console.Write("Nuevo Nombre: ");
        string nuevoNombre = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(nuevoNombre))
        {
            bool actualizado = _servicio.ActualizarCiudad(id, nuevoNombre);

            if (actualizado)
            {
                Console.WriteLine("✅ Ciudad actualizada con éxito.");
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

