using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System;

namespace SistemaGestorV.Application.Services;

public class TerceroService
{
    private readonly ITerceroRepository _repo;

    public TerceroService(ITerceroRepository repo)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    public IEnumerable<Tercero> ObtenerTodos()
    {
        return _repo.ObtenerTodos();
    }

    public Tercero ObtenerPorId(int id)
    {
        return _repo.ObtenerPorId(id);
    }

    public IEnumerable<Tercero> ObtenerPorTipo(int tipoTerceroId)
    {
        return _repo.ObtenerPorTipo(tipoTerceroId);
    }

    public void CrearTercero(Tercero tercero)
    {
        if (tercero == null)
            throw new ArgumentNullException(nameof(tercero));

        ValidarTercero(tercero);
        _repo.Crear(tercero);
    }

    public void ActualizarTercero(Tercero tercero)
    {
        if (tercero == null)
            throw new ArgumentNullException(nameof(tercero));

        if (tercero.Id <= 0)
            throw new ArgumentException("ID de tercero inválido");

        ValidarTercero(tercero);
        _repo.Actualizar(tercero);
    }

    public void ActualizarTercero(int id, string nuevoNombre, string nuevoCorreo, string nuevoTelefono)
    {
        var tercero = _repo.ObtenerPorId(id);
        if (tercero == null)
            throw new ArgumentException($"No se encontró un tercero con el ID {id}");

        tercero.Nombre = nuevoNombre;
        tercero.Email = nuevoCorreo;

        if (!string.IsNullOrWhiteSpace(nuevoTelefono))
        {
            if (tercero.Telefonos == null)
                tercero.Telefonos = new List<Telefono>();

            if (tercero.Telefonos.Any())
                tercero.Telefonos.First().Numero = nuevoTelefono;
            else
                tercero.Telefonos.Add(new Telefono { Numero = nuevoTelefono, Tipo = "Principal" });
        }

        ActualizarTercero(tercero);
    }

    public void EliminarTercero(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID de tercero inválido");

        _repo.Eliminar(id);
    }

    public void MostrarTodos()
    {
        var terceros = ObtenerTodos();
        Console.WriteLine("\n--- LISTA DE TERCEROS ---");
        
        foreach (var t in terceros)
        {
            Console.WriteLine($"ID: {t.Id}, Nombre: {t.Nombre} {t.Apellidos}, Tipo: {t.TipoTerceroDescripcion}, Email: {t.Email}");
            
            if (t.Telefonos?.Count > 0)
            {
                Console.WriteLine("   Teléfonos:");
                foreach (var tel in t.Telefonos)
                {
                    Console.WriteLine($"    - {tel.Numero} ({tel.Tipo})");
                }
            }
            
            switch (t.TipoTerceroId)
            {
                case 1 when t.Cliente != null:
                    Console.WriteLine($"   Tipo: Cliente, F.Nac: {t.Cliente.FechaNac:yyyy-MM-dd}, F.Informa: {t.Cliente.FechaInforma:yyyy-MM-dd}");
                    break;
                case 2 when t.Empleado != null:
                    Console.WriteLine($"   Tipo: Empleado, Salario: {t.Empleado.SalarioBase:C}, Ingreso: {t.Empleado.FechaIngreso:yyyy-MM-dd}");
                    break;
                case 3 when t.Proveedor != null:
                    Console.WriteLine($"   Tipo: Proveedor, Dcto: {t.Proveedor.Scto}%, Día Pago: {t.Proveedor.DiaPago}");
                    break;
            }
        }
    }

    private void ValidarTercero(Tercero tercero)
    {
        if (string.IsNullOrWhiteSpace(tercero.Nombre))
            throw new ArgumentException("El nombre del tercero es requerido");

        if (string.IsNullOrWhiteSpace(tercero.Apellidos))
            throw new ArgumentException("Los apellidos del tercero son requeridos");

        if (tercero.TipoTerceroId < 1 || tercero.TipoTerceroId > 3)
            throw new ArgumentException("Tipo de tercero inválido");

        switch (tercero.TipoTerceroId)
        {
            case 1 when tercero.Cliente == null:
                throw new ArgumentException("Datos de cliente son requeridos");
            case 2 when tercero.Empleado == null:
                throw new ArgumentException("Datos de empleado son requeridos");
            case 3 when tercero.Proveedor == null:
                throw new ArgumentException("Datos de proveedor son requeridos");
        }
    }

    public bool ExisteTerceroConId(int id)
    {
        return _repo.ObtenerPorId(id) != null;
    }
}