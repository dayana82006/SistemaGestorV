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

        public IEnumerable<Plan> ObtenerTodos()
        {
            var planes = new List<Plan>();
            
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT id, nombre, fechaInicio, fechaFin, dcto FROM planes";
                    
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
                throw; 
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
                        
                        if (plan.ProductosAsociados != null)
                        {
                            EliminarProductosAsociados(plan.Id);
                            
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
        
        public void Eliminar(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    EliminarProductosAsociados(id);
                    
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
        
        private List<string> ObtenerProductosAsociados(int planId)
        {
            var productos = new List<string>();
            
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    
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
                
            }
            
            return productos;
        }
       
        private void GuardarProductosAsociados(int planId, List<string> productos)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    foreach (var productoNombre in productos)
                    {
                        int productoId = ObtenerProductoIdPorNombre(connection, productoNombre);
                        
                        if (productoId > 0)
                        {
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
            
            return -1; 
        }
    }
}