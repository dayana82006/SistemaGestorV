
using System;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Repositories;
using SistemaGestorV.Domain.Factory;

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

    public ITerceroRepository CrearTerceroRepository()
    {
        return new MySqlTerceroRepository(_connectionString);
    }

        public IPlanesRepository CrearPlanesRepository()
        {
            return new MySqlPlanesRepository(_connectionString);
        }

    public ICompraRepository CrearCompraRepository()
    {
        return new ImpCompraRepository(_connectionString);
    }
    public IDetalleCompraRepository CrearDetalleCompraRepository()
    {
        return new ImpDetalleCompraRepository(_connectionString);
    }
    public IPaisRepository CrearPaisRepository()
    {
        return new ImpPaisRepository(_connectionString);
    }
    public IEpsRepository CrearEpsRepository()
    {
        return new ImpEpsRepository(_connectionString);
    }
    public ImpArlRepository CrearArlRepository()
    {
        return new ImpArlRepository(_connectionString);
    }
    public ImpTipoDocumentoRepository CrearTipoDocumentoRepository()
    {
        return new ImpTipoDocumentoRepository(_connectionString);
    }
    public ImpRegionRepository CrearRegionRepository()
    {
        return new ImpRegionRepository(_connectionString);
    }
    public ImpCiudadRepository CrearCiudadRepository()
    {
        return new ImpCiudadRepository(_connectionString);
    }

    public ImpDireccionRepository CrearDireccionRepository()
    {
        return new ImpDireccionRepository(_connectionString);
    }

    public IEmpresaRepository CrearEmpresaRepository()
    {
        return new ImpEmpresaRepository(_connectionString);
    }
}