using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Mysql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace SistemaGestorV.Infrastructure.Repositories
{
    public class ImpRegionRepository : IGenericRepository<Region>, IRegionRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpRegionRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public IEnumerable<Region> ObtenerTodos()
        {
            var regiones = new List<Region>();
            var connection = _conexion.ObtenerConexion();

            try
            {
                string query = "SELECT id, nombre, paisId FROM region";
                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    regiones.Add(new Region
                    {
                        id = Convert.ToInt32(reader["id"]),
                        nombre = reader.GetString("nombre"),
                        paisId = Convert.ToInt32(reader["paisId"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener todas las regiones: {ex.Message}");
            }

            return regiones;
        }

        public void Crear(Region region)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();

                // Validar si el país existe
                if (!ExistePais(region.paisId, connection))
                {
                    Console.WriteLine("❌ El país con ese ID no existe. Debe registrarlo primero.");
                    return;
                }

                string query = "INSERT INTO region (id, nombre, paisId) VALUES (@id, @nombre, @paisId)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", region.id);
                cmd.Parameters.AddWithValue("@nombre", region.nombre);
                cmd.Parameters.AddWithValue("@paisId", region.paisId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear la región: {ex.Message}");
            }
        }

        public void Actualizar(Region region)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();

                // Validar si el país existe
                if (!ExistePais(region.paisId, connection))
                {
                    Console.WriteLine("❌ El país con ese ID no existe. Debe registrarlo primero.");
                    return;
                }

                string query = "UPDATE region SET nombre = @nombre, paisId = @paisId WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", region.id);
                cmd.Parameters.AddWithValue("@nombre", region.nombre);
                cmd.Parameters.AddWithValue("@paisId", region.paisId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar la región: {ex.Message}");
            }
        }

        public void Eliminar(string id)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "DELETE FROM region WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar la región: {ex.Message}");
            }
        }

        IEnumerable<Region> IGenericRepository<Region>.ObtenerTodos()
        {
            return ObtenerTodos();
        }

        public Region? ObtenerPorId(string id)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "SELECT id, nombre, paisId FROM region WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Region
                    {
                        id = Convert.ToInt32(reader["id"]),
                        nombre = reader.GetString("nombre"),
                        paisId = Convert.ToInt32(reader["paisId"])
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener región por ID: {ex.Message}");
            }

            return null;
        }

        // Método privado para verificar si el país existe
        private bool ExistePais(int idPais, MySqlConnection connection)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM pais WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", idPais);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al validar existencia del país: {ex.Message}");
                return false;
            }
        }
    }
}
