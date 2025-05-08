using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Mysql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace SistemaGestorV.Infrastructure.Repositories
{
    public class ImpProductoRepository : IGenericRepository<Producto>, IProductoRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpProductoRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public void Actualizar(Producto producto)
        {
            try
            {
                using var connection = _conexion.ObtenerConexion();
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                string query = "UPDATE productos SET nombre = @nombre, stock = @stock WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@stock", producto.Stock);
                cmd.Parameters.AddWithValue("@id", producto.Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar producto: {ex.Message}");
            }
        }

        public void Crear(Producto producto)
        {
            try
            {
                using var connection = _conexion.ObtenerConexion();
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                string query = "INSERT INTO productos (nombre, stock) VALUES (@nombre, @stock)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@stock", producto.Stock);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear producto: {ex.Message}");
            }
        }

        public void Eliminar(string id)
        {
            try
            {
                using var connection = _conexion.ObtenerConexion();
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                string query = "DELETE FROM productos WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar producto: {ex.Message}");
            }
        }

        public Producto? ObtenerPorId(string id)
        {
            try
            {
                using var connection = _conexion.ObtenerConexion();
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                string query = "SELECT id, nombre, stock FROM productos WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Producto
                    {
                        Id = reader.GetString("id"),
                        Nombre = reader.GetString("nombre"),
                        Stock = reader.GetInt32("stock")
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener producto por ID: {ex.Message}");
            }

            return null;
        }

        public List<Producto> ObtenerTodos()
        {
            var productos = new List<Producto>();

            try
            {
                using var connection = _conexion.ObtenerConexion();
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                string query = "SELECT id, nombre, stock FROM productos";
                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productos.Add(new Producto
                    {
                        Id = reader.GetString("id"),
                        Nombre = reader.GetString("nombre"),
                        Stock = reader.GetInt32("stock")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener todos los productos: {ex.Message}");
            }

            return productos;
        }

        IEnumerable<Producto> IGenericRepository<Producto>.ObtenerTodos()
        {
            return ObtenerTodos();
        }
    }
}
