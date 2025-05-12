using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Mysql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace SistemaGestorV.Infrastructure.Repositories
{
    public class ImpCiudadRepository : IGenericRepository<Ciudad>, ICiudadRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpCiudadRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public IEnumerable<Ciudad> ObtenerTodos()
        {
            var ciudades = new List<Ciudad>();
            var connection = _conexion.ObtenerConexion();

            try
            {
                string query = "SELECT id, nombre, regionId FROM ciudad"; // CAMBIO AQUI
                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ciudades.Add(new Ciudad
                    {
                        id = Convert.ToInt32(reader["id"]),
                        nombre = reader.GetString("nombre"),
                        regionId = Convert.ToInt32(reader["regionId"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener todas las ciudades: {ex.Message}");
            }

            return ciudades;
        }

        public void Crear(Ciudad ciudad)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();

                if (!ExisteRegion(ciudad.regionId, connection))
                {
                    Console.WriteLine("❌ La región con ese ID no existe. Debe registrarlo primero.");
                    return;
                }

                string query = "INSERT INTO ciudad (id, nombre, regionId) VALUES (@id, @nombre, @regionId)"; // CAMBIO AQUI
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", ciudad.id);
                cmd.Parameters.AddWithValue("@nombre", ciudad.nombre);
                cmd.Parameters.AddWithValue("@regionId", ciudad.regionId);
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
                string query = "UPDATE ciudad SET nombre = @nombre, regionId = @regionId WHERE id = @id"; // CAMBIO AQUI
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", ciudad.id);
                cmd.Parameters.AddWithValue("@nombre", ciudad.nombre);
                cmd.Parameters.AddWithValue("@regionId", ciudad.regionId);
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
                string query = "DELETE FROM ciudad WHERE id = @id"; // CAMBIO AQUI
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar la ciudad: {ex.Message}");
            }
        }

        public Ciudad ObtenerPorId(string id)
        {
            Ciudad ciudad = null;
            var connection = _conexion.ObtenerConexion();

            try
            {
                string query = "SELECT id, nombre, regionId FROM ciudad WHERE id = @id"; // CAMBIO AQUI
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    ciudad = new Ciudad
                    {
                        id = Convert.ToInt32(reader["id"]),
                        nombre = reader.GetString("nombre"),
                        regionId = Convert.ToInt32(reader["regionId"])
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener la ciudad por ID: {ex.Message}");
            }

            return ciudad;
        }

        private bool ExisteRegion(int regionId, MySqlConnection connection)
        {
            string query = "SELECT COUNT(*) FROM region WHERE id = @regionId";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@regionId", regionId);
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }
    }
}
