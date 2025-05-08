using System;
using SistemaGestorV.Domain.Factory;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Repositories;

namespace SistemaGestorV.Infrastructure.Mysql;

    public class MySqlDbFactory : IDbFactory
    {
        private readonly string _connectionString;

        public MySqlDbFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IProductoRepository CrearProductoRepository()
        {
            return new ImpProductoRepository(_connectionString);
        }


       
    }

