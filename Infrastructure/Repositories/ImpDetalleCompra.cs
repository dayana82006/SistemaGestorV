using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Mysql;
using MySql.Data.MySqlClient;

namespace SistemaGestorV.Infrastructure.Repositories
{
    public class ImpDetalleCompraRepository : IRepositoryInt<DetalleCompra>, IDetalleCompraRepository
    {
        private readonly ConexionSingleton _conexion;

        public ImpDetalleCompraRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public List<DetalleCompra> ObtenerTodos()
        {
            var detalles = new List<DetalleCompra>();
            var connection = _conexion.ObtenerConexion();
            string query = "SELECT id, fecha, productoId, cantidad, valor, compraId FROM detalle_compra";

            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                detalles.Add(new DetalleCompra
                {
                    Id = reader.GetInt32(0),
                    Fecha = reader.GetDateTime(1),
                    ProductoId = reader.GetString(2),
                    Cantidad = reader.GetInt32(3),
                    Valor = reader.GetDouble(4),
                    CompraId = reader.GetInt32(5)
                });
            }

            return detalles;
        }

        public DetalleCompra? ObtenerPorId(int id)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "SELECT id, fecha, productoId, cantidad, valor, compraId FROM detalle_compra WHERE id = @id";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new DetalleCompra
                {
                    Id = reader.GetInt32(0),
                    Fecha = reader.GetDateTime(1),
                    ProductoId = reader.GetString(2),
                    Cantidad = reader.GetInt32(3),
                    Valor = reader.GetDouble(4),
                    CompraId = reader.GetInt32(5)
                };
            }

            return null;
        }

        public void Crear(DetalleCompra detalle)
        {
            var connection = _conexion.ObtenerConexion();
            string query = @"INSERT INTO detalle_compra (fecha, productoId, cantidad, valor, compraId)
                             VALUES (@fecha, @productoId, @cantidad, @valor, @compraId)";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@fecha", detalle.Fecha);
            cmd.Parameters.AddWithValue("@productoId", detalle.ProductoId);
            cmd.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
            cmd.Parameters.AddWithValue("@valor", detalle.Valor);
            cmd.Parameters.AddWithValue("@compraId", detalle.CompraId);
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(DetalleCompra detalle)
        {
            var connection = _conexion.ObtenerConexion();
            string query = @"UPDATE detalle_compra 
                             SET fecha = @fecha, productoId = @productoId, cantidad = @cantidad, 
                                 valor = @valor, compraId = @compraId 
                             WHERE id = @id";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", detalle.Id);
            cmd.Parameters.AddWithValue("@fecha", detalle.Fecha);
            cmd.Parameters.AddWithValue("@productoId", detalle.ProductoId);
            cmd.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
            cmd.Parameters.AddWithValue("@valor", detalle.Valor);
            cmd.Parameters.AddWithValue("@compraId", detalle.CompraId);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "DELETE FROM detalle_compra WHERE id = @id";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex) when (ex.Number == 1451)
            {
                throw new InvalidOperationException("No se puede eliminar este detalle porque está relacionado con otras entidades.");
            }
        }

        IEnumerable<DetalleCompra> IRepositoryInt<DetalleCompra>.ObtenerTodos() => ObtenerTodos();
    }
}
