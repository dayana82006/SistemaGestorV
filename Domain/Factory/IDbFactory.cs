
using System;
using SistemaGestorV.Domain.Ports;
using SistemaGestorV.Infrastructure.Repositories;
using SistemaGestorV.Infrastructure.Mysql;



namespace SistemaGestorV.Domain.Factory;

public interface IDbFactory
{
    ITerceroRepository CrearTerceroRepository();
    IProductoRepository CrearProductoRepository();
    IPlanesRepository CrearPlanesRepository();

    ICompraRepository CrearCompraRepository();
    IDetalleCompraRepository CrearDetalleCompraRepository();
    IPaisRepository CrearPaisRepository();
    IEpsRepository CrearEpsRepository();
    ImpArlRepository CrearArlRepository();
    ImpTipoDocumentoRepository CrearTipoDocumentoRepository();
    ImpCiudadRepository CrearCiudadRepository();
    ImpRegionRepository CrearRegionRepository();
    ImpDireccionRepository CrearDireccionRepository();
    IEmpresaRepository CrearEmpresaRepository();
}