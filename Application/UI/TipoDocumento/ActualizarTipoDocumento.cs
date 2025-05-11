using System;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.TipoDocumento;

public class ActualizarTipoDocumento
{
    private readonly TipoDocumentoService _servicio;

    public ActualizarTipoDocumento(TipoDocumentoService servicio)
    {
        _servicio = servicio;
    }

public void Ejecutar()
{
    Console.Write("Id Tipo de documento a actualizar: ");
    string id = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(id))
    {
        Console.Write("Nueva Descripcion: ");
        string nuevoDescripcion = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(nuevoDescripcion))
        {
            bool actualizado = _servicio.ActualizarTipoDocumento(id, nuevoDescripcion);

            if (actualizado)
            {
                Console.WriteLine("✅ Tipo de documento actualizado con éxito.");
            }
        }
        else
        {
            Console.WriteLine("❌ Descripcion inválida.");
        }
    }
    else
    {
        Console.WriteLine("❌ ID no válido.");
    }
}
}

