using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Mysql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using SistemaGestorV.Application.UI;

namespace SistemaGestorV.Infrastructure.Repositories 
{ 
    public class MySqlPlanesRepository : IGenericRepository<Plan>, IPlanesRepository
    {
        private readonly ConexionSingleton _conexion;

        public MySqlPlanesRepository(string connectionString)
        {
           _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public List<Plan> ObtenerTodos()
        {
            var planes = new List<Plan>();
            var connection = _conexion.ObtenerConexion();
            try
            {
                string query = "SELECT id, nombre, fechaInicio, fechaFin, dcto FROM planes";
                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    planes.Add(new Plan
                    {
                        Id = Convert.ToInt32(reader.GetString("id")),
                        Nombre = reader.GetString("nombre"),
                        FechaInicio = reader.GetDateTime("fechaInicio"),
                        FechaFin = reader.GetDateTime("fechaFin"),
                        Descuento = (double)reader.GetDecimal("Dcto")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener todos los planes: {ex.Message}");
            }

            return planes;
        }

        public void Crear(Plan plan)
        {
            
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "INSERT INTO planes (id, nombre, fechaInicio, fechaFin, dcto) VALUES (@id, @nombre, @fechaInicio, @fechaFin, @dcto)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", plan.Id);
                cmd.Parameters.AddWithValue("@nombre", plan.Nombre);
                cmd.Parameters.AddWithValue("@fechaInicio", plan.FechaInicio);
                cmd.Parameters.AddWithValue("@fechaFin", plan.FechaFin);
                cmd.Parameters.AddWithValue("@dcto", plan.Descuento);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el plan: {ex.Message}");
            }
            
        }
        public void Actualizar(Plan plan)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "UPDATE planes SET nombre = @nombre, fechaInicio = @fechaInicio, fechaFin = @fechaFin, dcto = @dcto WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", plan.Id);
                cmd.Parameters.AddWithValue("@nombre", plan.Nombre);
                cmd.Parameters.AddWithValue("@fechaInicio", plan.FechaInicio);
                cmd.Parameters.AddWithValue("@fechaFin", plan.FechaFin);
                cmd.Parameters.AddWithValue("@dcto", plan.Descuento);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el plan: {ex.Message}");
            }
        }

        public void Eliminar(string id)
        {
            try
            {
                var connection = _conexion.ObtenerConexion();
                string query = "DELETE FROM planes WHERE id = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el plan: {ex.Message}");
            }
        }
        IEnumerable<Plan> IGenericRepository<Plan>.ObtenerTodos()
    {
        return ObtenerTodos();

    }

        public Plan ObtenerPorId(string id)
    {
        Plan plan = null;
        try
        {
            var connection = _conexion.ObtenerConexion();
            string query = "SELECT id, nombre, fechaInicio, fechaFin, dcto FROM planes WHERE id = @id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                plan = new Plan
                {
                    Id = Convert.ToInt32(reader.GetString("id")),
                    Nombre = reader.GetString("nombre"),
                    FechaInicio = reader.GetDateTime("fechaInicio"),
                    FechaFin = reader.GetDateTime("fechaFin"),
                    Descuento = (double)reader.GetDecimal("dcto")
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener el plan por ID: {ex.Message}");
        }

        return plan;
    }


    }
}

    