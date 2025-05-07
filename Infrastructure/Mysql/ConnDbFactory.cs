using System;
using MiAppHexagonal.Domain.Factory;
using MiAppHexagonal.Domain.Ports;
//using MiAppHexagonal.Infrastructure.Repositories;

namespace MiAppHexagonal.Infrastructure.Mysql;

    public class MySqlDbFactory : IDbFactory
    {
        private readonly string _connectionString;

        public MySqlDbFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

       
    }

