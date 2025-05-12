using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Mysql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using InventoryManagement.Domain.Entities;

namespace SistemaGestorV.Infrastructure.Repositories
{
    public class ImpTipoDocumentoRepository : IGenericRepository<Tipo_documento>, ITipoDocumentoRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpTipoDocumentoRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public List<Tipo_documento> ObtenerTodos()
        {
            var tipo_documento = new List<Tipo_documento>();
            var connection = _conexion.ObtenerConexion();
            try
            {
                string query = "SELECT id, descripcion FROM tipo_documento";
                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tipo_documento.Add(new Tipo_documento
                    {
                        id = Convert.ToInt32(reader["id"]),
                        descripcion = reader.GetString("descripcion"),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener todos los tipos de documento: {ex.Message}");
            }

            return tipo_documento;
        }
         public void Crear(Tipo_documento tipo_documento)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "INSERT INTO tipo_documento (id, descripcion) VALUES (@id, @descripcion)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", tipo_documento.id);
                cmd.Parameters.AddWithValue("@descripcion", tipo_documento.descripcion);
            

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear el tipo de documento: {ex.Message}");
            }
        }
        public void Actualizar(Tipo_documento tipo_documento)
        {
            try
            {
                 
                var connection = _conexion.ObtenerConexion();
                string query = "UPDATE tipo_documento SET descripcion = @descripcion WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", tipo_documento.id);
                cmd.Parameters.AddWithValue("@descripcion", tipo_documento.descripcion);
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar el tipo de documento: {ex.Message}");
            }
        }


        public void Eliminar(string id)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "DELETE FROM tipo_documento WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar tipo de documento: {ex.Message}");
            }
        }
    IEnumerable<Tipo_documento> IGenericRepository<Tipo_documento>.ObtenerTodos()
    {
        return ObtenerTodos();

    }
public Tipo_documento? ObtenerPorId(string id)
{
    try
    {
        var connection = _conexion.ObtenerConexion();
        string query = "SELECT id, descripcion FROM tipo_documento WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Tipo_documento
            {
                id = Convert.ToInt32(reader["id"]),
                descripcion = reader.GetString("descripcion"),
            };
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error al obtener el tipo de documento por ID: {ex.Message}");
    }
    
    return null;
}

    }
}
