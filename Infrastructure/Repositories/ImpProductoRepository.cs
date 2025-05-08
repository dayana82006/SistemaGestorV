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

        public List<Producto> ObtenerTodos()
        {
            var productos = new List<Producto>();
            var connection = _conexion.ObtenerConexion();
            try
            {
                string query = "SELECT id, nombre, stock FROM productos";
                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productos.Add(new Producto
                    {
                        id = reader.GetString("id"),
                        nombre = reader.GetString("nombre"),
                        stock = reader.GetInt32("stock")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener todos los productos: {ex.Message}");
            }

            return productos;
        }
         public void Crear(Producto producto)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "INSERT INTO productos (id, nombre, stock, stockMin, stockMax,createdAt, updatedAt, barcode ) VALUES (@id, @nombre, @stock, @stockMin, @stockMax, @createdAt, @updatedAt, @barcode)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", producto.id);
                cmd.Parameters.AddWithValue("@nombre", producto.nombre);
                cmd.Parameters.AddWithValue("@stock", producto.stock);
                cmd.Parameters.AddWithValue("@stockMin", producto.stockMin);
                cmd.Parameters.AddWithValue("@stockMax", producto.stockMax);
                cmd.Parameters.AddWithValue("@createdAt", producto.createdAt);
                cmd.Parameters.AddWithValue("@updatedAt", producto.updatedAt);
                cmd.Parameters.AddWithValue("@barcode", producto.barcode);
            

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear producto: {ex.Message}");
            }
        }
        public void Actualizar(Producto producto)
        {
            try
            {
                 
                var connection = _conexion.ObtenerConexion();
                string query = "UPDATE productos SET nombre = @nombre, stock = @stock WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", producto.id);
                cmd.Parameters.AddWithValue("@nombre", producto.nombre);
                cmd.Parameters.AddWithValue("@stock", producto.stock);
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar producto: {ex.Message}");
            }
        }


        public void Eliminar(string id)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
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
    IEnumerable<Producto> IGenericRepository<Producto>.ObtenerTodos()
    {
        return ObtenerTodos();

    }
public Producto? ObtenerPorId(string id)
{
    try
    {
        var connection = _conexion.ObtenerConexion();
        string query = "SELECT id, nombre, stock FROM productos WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Producto
            {
                id = reader.GetString("id"),
                nombre = reader.GetString("nombre"),
                stock = reader.GetInt32("stock")
            };
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error al obtener producto por ID: {ex.Message}");
    }

    // Si no encontró el producto o hubo un error
    return null;
}

    }
}
