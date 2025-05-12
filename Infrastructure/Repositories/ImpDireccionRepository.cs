using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Mysql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using InventoryManagement.Domain.Entities;

namespace SistemaGestorV.Infrastructure.Repositories
{
    public class ImpDireccionRepository : IGenericRepository<Direccion>, IDireccionRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpDireccionRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public IEnumerable<Direccion> ObtenerTodos()
        {
            var direcciones = new List<Direccion>();
            var connection = _conexion.ObtenerConexion();

            try
            {
                string query = "SELECT id, ciudadId, calleNumero, calleNombre FROM direccion";
                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    direcciones.Add(new Direccion
                    {
                        id = Convert.ToInt32(reader["id"]),
                        ciudadId = Convert.ToInt32(reader["ciudadId"]),
                        calleNumero = reader.GetString("calleNumero"), // Cambié esto a string
                        calleNombre = reader.GetString("calleNombre")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener todas las direcciones: {ex.Message}");
            }

            return direcciones;
        }

        public void Crear(Direccion direccion)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();

                // Validar si la ciudad existe
                if (!ExisteCiudad(direccion.ciudadId, connection))
                {
                    Console.WriteLine("❌ La ciudad con ese ID no existe. Debe registrarla primero.");
                    return;
                }

                string query = "INSERT INTO direccion (id, ciudadId, calleNumero, calleNombre) VALUES (@id, @ciudadId, @calleNumero, @calleNombre)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", direccion.id);
                cmd.Parameters.AddWithValue("@ciudadId", direccion.ciudadId);
                cmd.Parameters.AddWithValue("@calleNumero", direccion.calleNumero);
                cmd.Parameters.AddWithValue("@calleNombre", direccion.calleNombre);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear la dirección: {ex.Message}");
            }
        }

        public void Actualizar(Direccion direccion)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();

                // Validar si la ciudad existe
                if (!ExisteCiudad(direccion.ciudadId, connection))
                {
                    Console.WriteLine("❌ La ciudad con ese ID no existe. Debe registrarla primero.");
                    return;
                }

                string query = "UPDATE direccion SET ciudadId = @ciudadId, calleNumero = @calleNumero, calleNombre = @calleNombre WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", direccion.id);
                cmd.Parameters.AddWithValue("@ciudadId", direccion.ciudadId);
                cmd.Parameters.AddWithValue("@calleNumero", direccion.calleNumero);
                cmd.Parameters.AddWithValue("@calleNombre", direccion.calleNombre);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar la dirección: {ex.Message}");
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "DELETE FROM direccion WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar la dirección: {ex.Message}");
            }
        }

        public void Eliminar(string id)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "DELETE FROM direccion WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar la dirección con ID string: {ex.Message}");
            }
        }

        public Direccion ObtenerPorId(string id)
        {
            Direccion direccion = null;
            var connection = _conexion.ObtenerConexion();

            try
            {
                string query = "SELECT id, ciudadId, calleNumero, calleNombre FROM direccion WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id); // Ahora se usa un string
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    direccion = new Direccion
                    {
                        id = Convert.ToInt32(reader["id"]),
                        ciudadId = Convert.ToInt32(reader["ciudadId"]),
                        calleNumero = reader.GetString("calleNumero"), // Cambié esto a string
                        calleNombre = reader.GetString("calleNombre")
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener la dirección por ID: {ex.Message}");
            }

            return direccion;
        }

        private bool ExisteCiudad(int ciudadId, MySqlConnection connection)
        {
            string query = "SELECT COUNT(*) FROM ciudad WHERE id = @ciudadId";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ciudadId", ciudadId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
    }
}
