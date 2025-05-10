using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Infrastructure.Mysql;
using MySql.Data.MySqlClient;
using System;


namespace SistemaGestorV.Infrastructure.Repositories;
public class ImpCompraRepository : IRepositoryInt<Compra>, ICompraRepository
{
    private readonly ConexionSingleton _conexion;
    public ImpCompraRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public List<Compra> ObtenerTodos()
    {
        var compra = new List<Compra>();
        var connection = _conexion.ObtenerConexion();
        string query = "SELECT id, terceroProvId, fecha, terceroEmpId  FROM compras";
        using var cmd = new MySqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            compra.Add(new Compra
            {
                Id = reader.GetInt32(0),
                TerceroProvId = reader.GetInt32(1),
                Fecha = reader.GetDateTime(2),
                TerceroEmpId = reader.GetInt32(3)
            });

        }
        return compra;
    }

    public void Crear(Compra compra)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO compras (terceroProvId, fecha, terceroEmpId, docCompra) VALUES (@terceroProvId, @fecha, @terceroEmpId, @docCompra)";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@terceroProvId", compra.TerceroProvId);
        cmd.Parameters.AddWithValue("@fecha", compra.Fecha);
        cmd.Parameters.AddWithValue("@terceroEmpId", compra.TerceroEmpId);
        cmd.Parameters.AddWithValue("@docCompra", compra.DocCompra);
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Compra compra)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE compras SET terceroProvId = @terceroProvId, fecha = @fecha, terceroEmpId = @terceroEmpId, docCompra = @docCompra WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", compra.Id);
        cmd.Parameters.AddWithValue("@terceroProvId", compra.TerceroProvId);
        cmd.Parameters.AddWithValue("@fecha", compra.Fecha);
        cmd.Parameters.AddWithValue("@terceroEmpId", compra.TerceroEmpId);
        cmd.Parameters.AddWithValue("@docCompra", compra.DocCompra);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM compras WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (MySqlException ex)
        {
            if (ex.Number == 1451)
            {
                throw new InvalidOperationException("No se puede eliminar el cliente porque está asociado a uno o más pedidos.");
            }
            else
            {
                throw;
            }
        }
    }

    IEnumerable<Compra> IRepositoryInt<Compra>.ObtenerTodos()
    {
        return ObtenerTodos();
    }

    public Compra? ObtenerPorId(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "SELECT id, terceroProvId, fecha, terceroEmpId FROM compras WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Compra
            {
                Id = reader.GetInt32(0),
                TerceroProvId = reader.GetInt32(1),
                Fecha = reader.GetDateTime(2),
                TerceroEmpId = reader.GetInt32(3)
            };
        }
       
        throw new Exception("Cliente no encontrado");
    }



}

