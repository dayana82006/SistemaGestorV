using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Mysql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using InventoryManagement.Domain.Entities;

namespace SistemaGestorV.Infrastructure.Repositories
{
    public class ImpEmpresaRepository : IGenericRepository<Empresa>, IEmpresaRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpEmpresaRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public IEnumerable<Empresa> ObtenerTodos()
        {
            var empresas = new List<Empresa>();
            var connection = _conexion.ObtenerConexion();

            try
            {
                string query = "SELECT id, nombre, direccionId, fechaReg FROM empresa";
                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    empresas.Add(new Empresa
                    {
                        id = reader["id"].ToString(),
                        nombre = reader.GetString("nombre"),
                        direccionId = Convert.ToInt32(reader["direccionId"]),
                        fechaReg = reader.GetDateTime("fechaReg")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener todas las empresas: {ex.Message}");
            }

            return empresas;
        }
        public void Crear(Empresa empresa)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();

                // Validar si la dirección existe
                if (!ExisteDireccion(empresa.direccionId, connection))
                {
                    Console.WriteLine("❌ La dirección con ese ID no existe. Debe registrarla primero.");
                    return;
                }

                string query = "INSERT INTO empresa (id, nombre, direccionId, fechaReg) VALUES (@id, @nombre, @direccionId, @fechaReg)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", empresa.id);
                cmd.Parameters.AddWithValue("@nombre", empresa.nombre);
                cmd.Parameters.AddWithValue("@direccionId", empresa.direccionId);
                cmd.Parameters.AddWithValue("@fechaReg", empresa.fechaReg);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear la empresa: {ex.Message}");
            }
        }

        public void Actualizar(Empresa empresa)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();

                // Validar si la dirección existe
                if (!ExisteDireccion(empresa.direccionId, connection))
                {
                    Console.WriteLine("❌ La dirección con ese ID no existe. Debe registrarla primero.");
                    return;
                }

                string query = "UPDATE empresa SET nombre = @nombre, direccionId = @direccionId, fechaReg = @fechaReg WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", empresa.id);
                cmd.Parameters.AddWithValue("@nombre", empresa.nombre);
                cmd.Parameters.AddWithValue("@direccionId", empresa.direccionId);
                cmd.Parameters.AddWithValue("@fechaReg", empresa.fechaReg);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar la empresa: {ex.Message}");
            }
        }

        public void Eliminar(string id)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "DELETE FROM empresa WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar la empresa (string ID): {ex.Message}");
            }
        }

        public Empresa ObtenerPorId(string id)
        {
            Empresa empresa = null;
            var connection = _conexion.ObtenerConexion();

            try
            {
                string query = "SELECT id, nombre, direccionId, fechaReg FROM empresa WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    empresa = new Empresa
                    {
                        id = reader["id"].ToString(),
                        nombre = reader.GetString("nombre"),
                        direccionId = Convert.ToInt32(reader["direccionId"]),
                        fechaReg = reader.GetDateTime("fechaReg")
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener la empresa por ID: {ex.Message}");
            }

            return empresa;
        }

        private bool ExisteDireccion(int direccionId, MySqlConnection connection)
        {
            string query = "SELECT COUNT(*) FROM direccion WHERE id = @direccionId";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@direccionId", direccionId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
    }
}
