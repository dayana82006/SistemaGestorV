using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Mysql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using InventoryManagement.Domain.Entities;

namespace SistemaGestorV.Infrastructure.Repositories
{
    public class ImpEpsRepository : IGenericRepository<Eps>, IEpsRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpEpsRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public List<Eps> ObtenerTodos()
        {
            var eps = new List<Eps>();
            var connection = _conexion.ObtenerConexion();
            try
            {
                string query = "SELECT id, nombre FROM eps";
                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    eps.Add(new Eps
                    {
                        id = Convert.ToInt32(reader["id"]),
                        nombre = reader.GetString("nombre"),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener todos los eps: {ex.Message}");
            }

            return eps;
        }
         public void Crear(Eps eps)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "INSERT INTO eps (id, nombre) VALUES (@id, @nombre)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", eps.id);
                cmd.Parameters.AddWithValue("@nombre", eps.nombre);
            

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear eps: {ex.Message}");
            }
        }
        public void Actualizar(Eps eps)
        {
            try
            {
                 
                var connection = _conexion.ObtenerConexion();
                string query = "UPDATE eps SET nombre = @nombre WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", eps.id);
                cmd.Parameters.AddWithValue("@nombre", eps.nombre);
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar eps: {ex.Message}");
            }
        }


        public void Eliminar(string id)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "DELETE FROM eps WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar eps: {ex.Message}");
            }
        }
    IEnumerable<Eps> IGenericRepository<Eps>.ObtenerTodos()
    {
        return ObtenerTodos();

    }
public Eps? ObtenerPorId(string id)
{
    try
    {
        var connection = _conexion.ObtenerConexion();
        string query = "SELECT id, nombre FROM eps WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Eps
            {
                id = Convert.ToInt32(reader["id"]),
                nombre = reader.GetString("nombre"),
            };
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error al obtener eps por ID: {ex.Message}");
    }
    
    return null;
}

    }
}
