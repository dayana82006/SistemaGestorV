using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Mysql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using InventoryManagement.Domain.Entities;

namespace SistemaGestorV.Infrastructure.Repositories
{
    public class ImpCiudadRepository : IGenericRepository<Ciudad>, ICiudadRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpCiudadRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public List<Ciudad> ObtenerTodos()
        {
            var ciudad = new List<Ciudad>();
            var connection = _conexion.ObtenerConexion();
            try
            {
                string query = "SELECT id, nombre FROM ciudad";
                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ciudad.Add(new Ciudad
                    {
                        id = Convert.ToInt32(reader["id"]),
                        nombre = reader.GetString("nombre"),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener todas las ciudades: {ex.Message}");
            }

            return ciudad;
        }
         public void Crear(Ciudad ciudad)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "INSERT INTO ciudad (id, nombre) VALUES (@id, @nombre)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", ciudad.id);
                cmd.Parameters.AddWithValue("@nombre", ciudad.nombre);
            

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear la ciudad: {ex.Message}");
            }
        }
        public void Actualizar(Ciudad ciudad)
        {
            try
            {
                 
                var connection = _conexion.ObtenerConexion();
                string query = "UPDATE ciudad SET nombre = @nombre WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", ciudad.id);
                cmd.Parameters.AddWithValue("@nombre", ciudad.nombre);
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar la ciudad: {ex.Message}");
            }
        }


        public void Eliminar(string id)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "DELETE FROM ciudad WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar ciudad: {ex.Message}");
            }
        }
    IEnumerable<Ciudad> IGenericRepository<Ciudad>.ObtenerTodos()
    {
        return ObtenerTodos();

    }
public Ciudad? ObtenerPorId(string id)
{
    try
    {
        var connection = _conexion.ObtenerConexion();
        string query = "SELECT id, nombre FROM ciudad WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Ciudad
            {
                id = Convert.ToInt32(reader["id"]),
                nombre = reader.GetString("nombre"),
            };
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error al obtener ciudad por ID: {ex.Message}");
    }
    
    return null;
}

    }
}
