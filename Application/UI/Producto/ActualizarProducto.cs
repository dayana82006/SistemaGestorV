using System;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.Producto;

public class ActualizarProducto
{
    private readonly ProductoService _servicio;

    public ActualizarProducto(ProductoService servicio)
    {
        _servicio = servicio;
    }

    public void Ejecutar()
    {
        Console.Write("Id Producto a actualizar: ");
        string id = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(id))
        {
            Console.Write("Nuevo Nombre: ");
            string nuevoNombre = Console.ReadLine();

            Console.Write("Cantidad en stock: ");
            if (int.TryParse(Console.ReadLine(), out int nuevoStock))
            {
                _servicio.ActualizarProducto(id, nuevoNombre, nuevoStock);
                Console.WriteLine("Producto Actualizado");
            }
            else
            {
                Console.WriteLine("Cantidad no válida.");
            }
        }
        else
        {
            Console.WriteLine("ID no válido");
        }
    }
}
