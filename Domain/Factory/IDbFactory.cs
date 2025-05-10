using System;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Repositories;
using SistemaGestorV.Infrastructure.Mysql;


namespace SistemaGestorV.Domain.Factory;

public interface IDbFactory
{
    ITerceroRepository CrearTerceroRepository();
    IProductoRepository CrearProductoRepository();
    ICompraRepository CrearCompraRepository();
    IDetalleCompraRepository CrearDetalleCompraRepository();
    
}
