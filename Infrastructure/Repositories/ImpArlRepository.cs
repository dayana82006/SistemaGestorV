using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Mysql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using InventoryManagement.Domain.Entities;

namespace SistemaGestorV.Infrastructure.Repositories
{
    public class ImpArlRepository : IGenericRepository<Arl>, IArlRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpArlRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public List<Arl> ObtenerTodos()
        {
            var arl = new List<Arl>();
            var connection = _conexion.ObtenerConexion();
            try
            {
                string query = "SELECT id, nombre FROM arl";
                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    arl.Add(new Arl
                    {
                        id = Convert.ToInt32(reader["id"]),
                        nombre = reader.GetString("nombre"),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener todos los arl: {ex.Message}");
            }

            return arl;
        }
         public void Crear(Arl arl)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "INSERT INTO arl (id, nombre) VALUES (@id, @nombre)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", arl.id);
                cmd.Parameters.AddWithValue("@nombre", arl.nombre);
            

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear arl: {ex.Message}");
            }
        }
        public void Actualizar(Arl arl)
        {
            try
            {
                 
                var connection = _conexion.ObtenerConexion();
                string query = "UPDATE arl SET nombre = @nombre WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", arl.id);
                cmd.Parameters.AddWithValue("@nombre", arl.nombre);
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar arl: {ex.Message}");
            }
        }


        public void Eliminar(string id)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "DELETE FROM arl WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar arl: {ex.Message}");
            }
        }
    IEnumerable<Arl> IGenericRepository<Arl>.ObtenerTodos()
    {
        return ObtenerTodos();

    }
public Arl? ObtenerPorId(string id)
{
    try
    {
        var connection = _conexion.ObtenerConexion();
        string query = "SELECT id, nombre FROM arl WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Arl
            {
                id = Convert.ToInt32(reader["id"]),
                nombre = reader.GetString("nombre"),
            };
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error al obtener arl por ID: {ex.Message}");
    }
    
    return null;
}

    }
}
