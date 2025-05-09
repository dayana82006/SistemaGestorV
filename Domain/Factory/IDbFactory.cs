<<<<<<< HEAD
using System;
using SistemaGestorV.Domain.Ports;

namespace SistemaGestorV.Domain.Factory;

public interface IDbFactory
{
   
  IProductoRepository CrearProductoRepository();

}
=======
namespace SistemaGestorV.Domain.Ports
{
    public interface IDbFactory
    {
        ITerceroRepository CrearTerceroRepository();
    }
}
>>>>>>> 40efe5fd4ebd427bbe8aab9888f57560a6d20a46
