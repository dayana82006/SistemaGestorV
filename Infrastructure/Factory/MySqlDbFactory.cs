using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Repositories;

namespace SistemaGestorV.Infrastructure.Factory
{
    public class MySqlDbFactory : IDbFactory
    {
        private readonly string _connectionString;

        public MySqlDbFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public ITerceroRepository CrearTerceroRepository()
        {
            return new MySqlTerceroRepository(_connectionString);
        }
    }
}