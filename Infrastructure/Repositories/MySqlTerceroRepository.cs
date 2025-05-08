using MySql.Data.MySqlClient;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System;
using System.Collections.Generic;

namespace SistemaGestorV.Infrastructure.Repositories
{
    public class MySqlTerceroRepository : ITerceroRepository
    {
        private readonly string _connectionString;

        public MySqlTerceroRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<Tercero> ObtenerTodos()
        {
            var terceros = new List<Tercero>();
            
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                
                var query = "SELECT * FROM terceros";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tercero = MapTerceroFromReader(reader);
                        if (tercero != null)
                        {
                            terceros.Add(tercero);
                        }
                    }
                }
                
                // Cargar datos específicos y teléfonos
                foreach (var tercero in terceros)
                {
                    if (tercero != null)
                    {
                        CargarDatosEspecificos(connection, tercero);
                        tercero.Telefonos = new List<Telefono>(ObtenerTelefonosPorTercero(tercero.Id));
                    }
                }
            }
            
            return terceros;
        }

        public Tercero? ObtenerPorId(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                
                var query = "SELECT * FROM terceros WHERE Id = @Id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var tercero = MapTerceroFromReader(reader);
                            if (tercero != null)
                            {
                                CargarDatosEspecificos(connection, tercero);
                                tercero.Telefonos = new List<Telefono>(ObtenerTelefonosPorTercero(id));
                            }
                            return tercero;
                        }
                    }
                }
            }
            
            return null;
        }

        public void Crear(Tercero tercero)
        {
            if (tercero == null) throw new ArgumentNullException(nameof(tercero));

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insertar tercero
                        var query = @"INSERT INTO terceros 
                                    (nombre, apellidos, email, tipoDoc_Id, tipoTercero_Id, DireccionId, empresa_ID) 
                                    VALUES (@Nombre, @Apellidos, @Email, @TipoDocId, @TipoTerceroId, @DireccionId, @EmpresaId);
                                    SELECT LAST_INSERT_ID();";
                        
                        using (var command = new MySqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Nombre", tercero.Nombre ?? string.Empty);
                            command.Parameters.AddWithValue("@Apellidos", tercero.Apellidos ?? string.Empty);
                            command.Parameters.AddWithValue("@Email", tercero.Email ?? string.Empty);
                            command.Parameters.AddWithValue("@TipoDocId", tercero.TipoDocumentoId);
                            command.Parameters.AddWithValue("@TipoTerceroId", tercero.TipoTerceroId);
                            command.Parameters.AddWithValue("@DireccionId", tercero.DireccionId);
                            command.Parameters.AddWithValue("@EmpresaId", tercero.EmpresaId ?? string.Empty);
                            
                            tercero.Id = Convert.ToInt32(command.ExecuteScalar());
                        }

                        // Insertar datos específicos según el tipo
                        switch (tercero.TipoTerceroId)
                        {
                            case 1 when tercero.Cliente != null:
                                InsertarCliente(connection, transaction, tercero);
                                break;
                            case 2 when tercero.Empleado != null:
                                InsertarEmpleado(connection, transaction, tercero);
                                break;
                            case 3 when tercero.Proveedor != null:
                                InsertarProveedor(connection, transaction, tercero);
                                break;
                        }

                        // Insertar teléfonos
                        if (tercero.Telefonos != null)
                        {
                            foreach (var telefono in tercero.Telefonos)
                            {
                                if (telefono != null)
                                {
                                    telefono.TerceroId = tercero.Id;
                                    AgregarTelefono(telefono);
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Actualizar(Tercero tercero)
        {
            if (tercero == null) throw new ArgumentNullException(nameof(tercero));

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Actualizar tercero
                        var query = @"UPDATE terceros SET 
                                    nombre = @Nombre, 
                                    apellidos = @Apellidos, 
                                    email = @Email, 
                                    tipoDoc_Id = @TipoDocId, 
                                    tipoTercero_Id = @TipoTerceroId, 
                                    DireccionId = @DireccionId, 
                                    empresa_ID = @EmpresaId 
                                    WHERE Id = @Id";
                        
                        using (var command = new MySqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Id", tercero.Id);
                            command.Parameters.AddWithValue("@Nombre", tercero.Nombre ?? string.Empty);
                            command.Parameters.AddWithValue("@Apellidos", tercero.Apellidos ?? string.Empty);
                            command.Parameters.AddWithValue("@Email", tercero.Email ?? string.Empty);
                            command.Parameters.AddWithValue("@TipoDocId", tercero.TipoDocumentoId);
                            command.Parameters.AddWithValue("@TipoTerceroId", tercero.TipoTerceroId);
                            command.Parameters.AddWithValue("@DireccionId", tercero.DireccionId);
                            command.Parameters.AddWithValue("@EmpresaId", tercero.EmpresaId ?? string.Empty);
                            
                            command.ExecuteNonQuery();
                        }

                        // Actualizar datos específicos según el tipo
                        switch (tercero.TipoTerceroId)
                        {
                            case 1 when tercero.Cliente != null:
                                ActualizarCliente(connection, transaction, tercero);
                                break;
                            case 2 when tercero.Empleado != null:
                                ActualizarEmpleado(connection, transaction, tercero);
                                break;
                            case 3 when tercero.Proveedor != null:
                                ActualizarProveedor(connection, transaction, tercero);
                                break;
                        }

                        // Actualizar teléfonos (eliminar todos y volver a insertar)
                        EliminarTelefonosPorTercero(tercero.Id);
                        if (tercero.Telefonos != null)
                        {
                            foreach (var telefono in tercero.Telefonos)
                            {
                                if (telefono != null)
                                {
                                    telefono.TerceroId = tercero.Id;
                                    AgregarTelefono(telefono);
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Eliminar(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Primero eliminar teléfonos
                        EliminarTelefonosPorTercero(id);

                        // Eliminar datos específicos según el tipo
                        var tercero = ObtenerPorId(id);
                        if (tercero != null)
                        {
                            switch (tercero.TipoTerceroId)
                            {
                                case 1:
                                    EliminarCliente(connection, transaction, id);
                                    break;
                                case 2:
                                    EliminarEmpleado(connection, transaction, id);
                                    break;
                                case 3:
                                    EliminarProveedor(connection, transaction, id);
                                    break;
                            }
                        }

                        // Finalmente eliminar el tercero
                        var query = "DELETE FROM terceros WHERE Id = @Id";
                        using (var command = new MySqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Id", id);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public IEnumerable<Tercero> ObtenerPorTipo(int tipoTerceroId)
        {
            var terceros = new List<Tercero>();
            
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                
                var query = "SELECT * FROM terceros WHERE tipoTercero_Id = @TipoTerceroId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TipoTerceroId", tipoTerceroId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tercero = MapTerceroFromReader(reader);
                            if (tercero != null)
                            {
                                terceros.Add(tercero);
                            }
                        }
                    }
                }
                
                // Cargar datos específicos y teléfonos
                foreach (var tercero in terceros)
                {
                    if (tercero != null)
                    {
                        CargarDatosEspecificos(connection, tercero);
                        tercero.Telefonos = new List<Telefono>(ObtenerTelefonosPorTercero(tercero.Id));
                    }
                }
            }
            
            return terceros;
        }

        public IEnumerable<Telefono> ObtenerTelefonosPorTercero(int terceroId)
        {
            var telefonos = new List<Telefono>();
            
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                
                var query = "SELECT * FROM tercero_telefonos WHERE tercero_id = @TerceroId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TerceroId", terceroId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            telefonos.Add(new Telefono
                            {
                                Id = reader.GetInt32("Id"),
                                Numero = reader.GetString("numero"),
                                Tipo = reader.GetString("tipo"),
                                TerceroId = terceroId
                            });
                        }
                    }
                }
            }
            
            return telefonos;
        }

        public void AgregarTelefono(Telefono telefono)
        {
            if (telefono == null) throw new ArgumentNullException(nameof(telefono));

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                
                var query = @"INSERT INTO tercero_telefonos 
                            (numero, tipo, tercero_id) 
                            VALUES (@Numero, @Tipo, @TerceroId)";
                
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Numero", telefono.Numero ?? string.Empty);
                    command.Parameters.AddWithValue("@Tipo", telefono.Tipo ?? string.Empty);
                    command.Parameters.AddWithValue("@TerceroId", telefono.TerceroId);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EliminarTelefonosPorTercero(int terceroId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                
                var query = "DELETE FROM tercero_telefonos WHERE tercero_id = @TerceroId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TerceroId", terceroId);
                    command.ExecuteNonQuery();
                }
            }
        }

        #region Métodos Privados

        private Tercero? MapTerceroFromReader(MySqlDataReader reader)
        {
            try
            {
                return new Tercero
                {
                    Id = reader.GetInt32("Id"),
                    Nombre = reader.GetString("nombre"),
                    Apellidos = reader.GetString("apellidos"),
                    Email = reader.GetString("email"),
                    TipoDocumentoId = reader.GetInt32("tipoDoc_Id"),
                    TipoTerceroId = reader.GetInt32("tipoTercero_Id"),
                    DireccionId = reader.GetInt32("DireccionId"),
                    EmpresaId = reader.GetString("empresa_ID")
                };
            }
            catch
            {
                return null;
            }
        }

        private void CargarDatosEspecificos(MySqlConnection connection, Tercero tercero)
        {
            if (tercero == null) return;

            switch (tercero.TipoTerceroId)
            {
                case 1:
                    tercero.Cliente = ObtenerCliente(connection, tercero.Id);
                    break;
                case 2:
                    tercero.Empleado = ObtenerEmpleado(connection, tercero.Id);
                    break;
                case 3:
                    tercero.Proveedor = ObtenerProveedor(connection, tercero.Id);
                    break;
            }
        }

        private Cliente? ObtenerCliente(MySqlConnection connection, int terceroId)
        {
            var query = "SELECT * FROM Cliente WHERE Tercero_Id = @TerceroId";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TerceroId", terceroId);
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Cliente
                        {
                            Id = reader.GetInt32("Id"),
                            TerceroId = terceroId,
                            FechaNac = reader.GetDateTime("FechaNac"),
                            FechaInforma = reader.GetDateTime("FechaInforma")
                        };
                    }
                }
            }
            return null;
        }

        private Empleado? ObtenerEmpleado(MySqlConnection connection, int terceroId)
        {
            var query = "SELECT * FROM Empleado WHERE Tercero_Id = @TerceroId";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TerceroId", terceroId);
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Empleado
                        {
                            Id = reader.GetInt32("Id"),
                            TerceroId = terceroId,
                            FechaIngreso = reader.GetDateTime("FechaIngreso"),
                            SalarioBase = reader.GetDouble("SalarioBase"),
                            EpsId = reader.GetInt32("Eps_Id"),
                            ArlId = reader.GetInt32("Arl_Id")
                        };
                    }
                }
            }
            return null;
        }

        private Proveedor? ObtenerProveedor(MySqlConnection connection, int terceroId)
        {
            var query = "SELECT * FROM Proveedor WHERE Tercero_Id = @TerceroId";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TerceroId", terceroId);
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Proveedor
                        {
                            Id = reader.GetInt32("Id"),
                            TerceroId = terceroId,
                            Scto = reader.GetDouble("Scto"),
                            DiaPago = reader.GetInt32("DiaPago")
                        };
                    }
                }
            }
            return null;
        }

        private void InsertarCliente(MySqlConnection connection, MySqlTransaction transaction, Tercero tercero)
        {
            if (tercero.Cliente == null) return;

            var query = @"INSERT INTO Cliente 
                        (Tercero_Id, FechaNac, FechaInforma) 
                        VALUES (@TerceroId, @FechaNac, @FechaInforma);
                        SELECT LAST_INSERT_ID();";
            
            using (var command = new MySqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@TerceroId", tercero.Id);
                command.Parameters.AddWithValue("@FechaNac", tercero.Cliente.FechaNac);
                command.Parameters.AddWithValue("@FechaInforma", tercero.Cliente.FechaInforma);
                
                tercero.Cliente.Id = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private void InsertarEmpleado(MySqlConnection connection, MySqlTransaction transaction, Tercero tercero)
        {
            if (tercero.Empleado == null) return;

            var query = @"INSERT INTO Empleado 
                        (Tercero_Id, FechaIngreso, SalarioBase, Eps_Id, Arl_Id) 
                        VALUES (@TerceroId, @FechaIngreso, @SalarioBase, @EpsId, @ArlId);
                        SELECT LAST_INSERT_ID();";
            
            using (var command = new MySqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@TerceroId", tercero.Id);
                command.Parameters.AddWithValue("@FechaIngreso", tercero.Empleado.FechaIngreso);
                command.Parameters.AddWithValue("@SalarioBase", tercero.Empleado.SalarioBase);
                command.Parameters.AddWithValue("@EpsId", tercero.Empleado.EpsId);
                command.Parameters.AddWithValue("@ArlId", tercero.Empleado.ArlId);
                
                tercero.Empleado.Id = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private void InsertarProveedor(MySqlConnection connection, MySqlTransaction transaction, Tercero tercero)
        {
            if (tercero.Proveedor == null) return;

            var query = @"INSERT INTO Proveedor 
                        (Tercero_Id, Scto, DiaPago) 
                        VALUES (@TerceroId, @Scto, @DiaPago);
                        SELECT LAST_INSERT_ID();";
            
            using (var command = new MySqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@TerceroId", tercero.Id);
                command.Parameters.AddWithValue("@Scto", tercero.Proveedor.Scto);
                command.Parameters.AddWithValue("@DiaPago", tercero.Proveedor.DiaPago);
                
                tercero.Proveedor.Id = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private void ActualizarCliente(MySqlConnection connection, MySqlTransaction transaction, Tercero tercero)
        {
            if (tercero.Cliente == null) return;

            var query = @"UPDATE Cliente SET 
                        FechaNac = @FechaNac, 
                        FechaInforma = @FechaInforma 
                        WHERE Tercero_Id = @TerceroId";
            
            using (var command = new MySqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@TerceroId", tercero.Id);
                command.Parameters.AddWithValue("@FechaNac", tercero.Cliente.FechaNac);
                command.Parameters.AddWithValue("@FechaInforma", tercero.Cliente.FechaInforma);
                
                command.ExecuteNonQuery();
            }
        }

        private void ActualizarEmpleado(MySqlConnection connection, MySqlTransaction transaction, Tercero tercero)
        {
            if (tercero.Empleado == null) return;

            var query = @"UPDATE Empleado SET 
                        FechaIngreso = @FechaIngreso, 
                        SalarioBase = @SalarioBase, 
                        Eps_Id = @EpsId, 
                        Arl_Id = @ArlId 
                        WHERE Tercero_Id = @TerceroId";
            
            using (var command = new MySqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@TerceroId", tercero.Id);
                command.Parameters.AddWithValue("@FechaIngreso", tercero.Empleado.FechaIngreso);
                command.Parameters.AddWithValue("@SalarioBase", tercero.Empleado.SalarioBase);
                command.Parameters.AddWithValue("@EpsId", tercero.Empleado.EpsId);
                command.Parameters.AddWithValue("@ArlId", tercero.Empleado.ArlId);
                
                command.ExecuteNonQuery();
            }
        }

        private void ActualizarProveedor(MySqlConnection connection, MySqlTransaction transaction, Tercero tercero)
        {
            if (tercero.Proveedor == null) return;

            var query = @"UPDATE Proveedor SET 
                        Scto = @Scto, 
                        DiaPago = @DiaPago 
                        WHERE Tercero_Id = @TerceroId";
            
            using (var command = new MySqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@TerceroId", tercero.Id);
                command.Parameters.AddWithValue("@Scto", tercero.Proveedor.Scto);
                command.Parameters.AddWithValue("@DiaPago", tercero.Proveedor.DiaPago);
                
                command.ExecuteNonQuery();
            }
        }

        private void EliminarCliente(MySqlConnection connection, MySqlTransaction transaction, int terceroId)
        {
            var query = "DELETE FROM Cliente WHERE Tercero_Id = @TerceroId";
            using (var command = new MySqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@TerceroId", terceroId);
                command.ExecuteNonQuery();
            }
        }

        private void EliminarEmpleado(MySqlConnection connection, MySqlTransaction transaction, int terceroId)
        {
            var query = "DELETE FROM Empleado WHERE Tercero_Id = @TerceroId";
            using (var command = new MySqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@TerceroId", terceroId);
                command.ExecuteNonQuery();
            }
        }

        private void EliminarProveedor(MySqlConnection connection, MySqlTransaction transaction, int terceroId)
        {
            var query = "DELETE FROM Proveedor WHERE Tercero_Id = @TerceroId";
            using (var command = new MySqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@TerceroId", terceroId);
                command.ExecuteNonQuery();
            }
        }

        #endregion
    }
}