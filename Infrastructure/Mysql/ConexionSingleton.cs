using System;
using MySql.Data.MySqlClient;

namespace MiAppHexagonal.Infrastructure.Mysql
{
    public class ConexionSingleton
    {
        private static ConexionSingleton? _instancia;
        private static readonly object _lock = new();

        private readonly string _connectionString;
        private MySqlConnection? _conexion;

        private ConexionSingleton(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static ConexionSingleton Instancia(string connectionString)
        {
            lock (_lock)
            {
                if (_instancia == null)
                {
                    _instancia = new ConexionSingleton(connectionString);
                }
                else if (_instancia._connectionString != connectionString)
                {
                    throw new InvalidOperationException("Ya se ha inicializado con una cadena de conexión diferente.");
                }

                return _instancia;
            }
        }

        public MySqlConnection ObtenerConexion()
        {
            _conexion ??= new MySqlConnection(_connectionString);

            if (_conexion.State != System.Data.ConnectionState.Open)
                _conexion.Open();

            return _conexion;
        }

        public void CerrarConexion()
        {
            if (_conexion != null && _conexion.State == System.Data.ConnectionState.Open)
                _conexion.Close();
        }
    }
}
