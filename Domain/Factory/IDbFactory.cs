using System;
using SistemaGestorV.Domain.Ports;

namespace SistemaGestorV.Domain.Factory;

public interface IDbFactory
{
   
  IProductoRepository CrearProductoRepository();

}
