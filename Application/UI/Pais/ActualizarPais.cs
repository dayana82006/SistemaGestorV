using System;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Pais;

public class ActualizarPais
{
    private readonly PaisService _servicio;

    public ActualizarPais(PaisService servicio)
    {
        _servicio = servicio;
    }

public void Ejecutar()
{
    Console.Write("Id Pais a actualizar: ");
    string id = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(id))
    {
        Console.Write("Nuevo Nombre: ");
        string nuevoNombre = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(nuevoNombre))
        {
            bool actualizado = _servicio.ActualizarPais(id, nuevoNombre);

            if (actualizado)
            {
                Console.WriteLine("✅ Pais actualizada con éxito.");
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

