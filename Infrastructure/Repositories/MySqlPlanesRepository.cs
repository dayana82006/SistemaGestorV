// Correcciones para MySqlPlanesRepository.cs

using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace SistemaGestorV.Infrastructure.Repositories
{
    public class MySqlPlanesRepository : IPlanesRepository
    {
        private readonly string _connectionString;

        public MySqlPlanesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 1. Corregir el tipo de retorno para que coincida con la interfaz
        public IEnumerable<Plan> ObtenerTodos()
        {
            var planes = new List<Plan>();
            
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT * FROM planes";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var plan = new Plan
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Nombre = reader["nombre"].ToString(),
                                    FechaInicio = Convert.ToDateTime(reader["fechaInicio"]),
                                    FechaFin = Convert.ToDateTime(reader["fechaFin"]),
                                    dcto = Convert.ToDouble(reader["dcto"])
                                };
                                
                                // Obtener productos asociados (esto depende de tu estructura de base de datos)
                                plan.ProductosAsociados = ObtenerProductosAsociados(plan.Id);
                                
                                planes.Add(plan);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener planes: {ex.Message}");
                throw; // Re-lanzar la excepción para que se maneje en capas superiores
            }
            
            return planes;
        }
        
        public Plan ObtenerPorId(string id)
        {
            if (!int.TryParse(id, out int idInt))
            {
                throw new ArgumentException("El ID debe ser un número entero válido.");
            }
            
            return ObtenerPorId(idInt);
        }
        
        // 2. Implementar el método ObtenerPorId para int
        public Plan ObtenerPorId(int id)
        {
            Plan plan = null;
            
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT * FROM planes WHERE id = @Id";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                plan = new Plan
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Nombre = reader["nombre"].ToString(),
                                    FechaInicio = Convert.ToDateTime(reader["fechaInicio"]),
                                    FechaFin = Convert.ToDateTime(reader["fechaFin"]),
                                    dcto = Convert.ToDouble(reader["dcto"])
                                };
                                
                                // Obtener productos asociados
                                plan.ProductosAsociados = ObtenerProductosAsociados(plan.Id);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el plan con ID {id}: {ex.Message}");
                throw;
            }
            
            return plan;
        }
        
        public void Crear(Plan plan)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"INSERT INTO planes (nombre, fechaInicio, fechaFin, dcto) 
                                    VALUES (@Nombre, @FechaInicio, @FechaFin, @dcto);
                                    SELECT LAST_INSERT_ID();";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", plan.Nombre);
                        command.Parameters.AddWithValue("@FechaInicio", plan.FechaInicio);
                        command.Parameters.AddWithValue("@FechaFin", plan.FechaFin);
                        command.Parameters.AddWithValue("@dcto", plan.dcto);
                        
                        int planId = Convert.ToInt32(command.ExecuteScalar());
                        plan.Id = planId;
                        
                        // Guardar productos asociados si existen
                        if (plan.ProductosAsociados != null && plan.ProductosAsociados.Any())
                        {
                            GuardarProductosAsociados(planId, plan.ProductosAsociados);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el plan: {ex.Message}");
                throw;
            }
        }
        
        public void Actualizar(Plan plan)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"UPDATE planes 
                                    SET nombre = @Nombre, 
                                        fechaInicio = @FechaInicio, 
                                        fechaFin = @FechaFin, 
                                        dcto = @dcto 
                                    WHERE id = @Id";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", plan.Id);
                        command.Parameters.AddWithValue("@Nombre", plan.Nombre);
                        command.Parameters.AddWithValue("@FechaInicio", plan.FechaInicio);
                        command.Parameters.AddWithValue("@FechaFin", plan.FechaFin);
                        command.Parameters.AddWithValue("@dcto", plan.dcto);
                        
                        command.ExecuteNonQuery();
                        
                        // Actualizar productos asociados
                        if (plan.ProductosAsociados != null)
                        {
                            // Eliminar asociaciones anteriores
                            EliminarProductosAsociados(plan.Id);
                            
                            // Crear nuevas asociaciones
                            if (plan.ProductosAsociados.Any())
                            {
                                GuardarProductosAsociados(plan.Id, plan.ProductosAsociados);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el plan con ID {plan.Id}: {ex.Message}");
                throw;
            }
        }
        
        public void Eliminar(string id)
        {
            if (!int.TryParse(id, out int idInt))
            {
                throw new ArgumentException("El ID debe ser un número entero válido.");
            }
            
            Eliminar(idInt);
        }
        
        // 3. Implementar el método Eliminar para int
        public void Eliminar(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    // Primero eliminar las asociaciones con productos
                    EliminarProductosAsociados(id);
                    
                    // Luego eliminar el plan
                    string query = "DELETE FROM planes WHERE id = @Id";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el plan con ID {id}: {ex.Message}");
                throw;
            }
        }
        
        // Método auxiliar para obtener productos asociados a un plan
        private List<string> ObtenerProductosAsociados(int planId)
        {
            var productos = new List<string>();
            
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    // Ajusta esta consulta según tu estructura de base de datos
                    string query = @"SELECT p.nombre 
                                    FROM productos p
                                    JOIN planproducto pp ON p.id = pp.productoId
                                    WHERE pp.planId = @PlanId";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PlanId", planId);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(reader["nombre"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener productos asociados al plan {planId}: {ex.Message}");
                // Aquí puedes decidir si lanzar la excepción o simplemente devolver la lista vacía
            }
            
            return productos;
        }
        
        // Método auxiliar para guardar productos asociados a un plan
        private void GuardarProductosAsociados(int planId, List<string> productos)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    foreach (var productoNombre in productos)
                    {
                        // Obtener ID del producto por su nombre
                        int productoId = ObtenerProductoIdPorNombre(connection, productoNombre);
                        
                        if (productoId > 0)
                        {
                            // Insertar la asociación
                            string query = @"INSERT INTO planproducto (planId, productoId) 
                                           VALUES (@PlanId, @ProductoId)";
                            
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@PlanId", planId);
                                command.Parameters.AddWithValue("@ProductoId", productoId);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar productos asociados al plan {planId}: {ex.Message}");
                throw;
            }
        }
        
        // Método auxiliar para eliminar las asociaciones de productos de un plan
        private void EliminarProductosAsociados(int planId)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "DELETE FROM planproducto WHERE planId = @PlanId";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PlanId", planId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar productos asociados al plan {planId}: {ex.Message}");
                throw;
            }
        }
        
        // Método auxiliar para obtener el ID de un producto por su nombre
        private int ObtenerProductoIdPorNombre(MySqlConnection connection, string nombre)
        {
            string query = "SELECT id FROM productos WHERE nombre = @Nombre LIMIT 1";
            
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", nombre);
                var result = command.ExecuteScalar();
                
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            }
            
            return -1; // Producto no encontrado
        }
    }
}