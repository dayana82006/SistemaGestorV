using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System.Linq;

namespace SistemaGestorV.Application.Services
{
    public class CompraService
    {
        private readonly ICompraRepository _repo;
        private readonly IDetalleCompraRepository _detalleRepo;

        public CompraService(ICompraRepository repo, IDetalleCompraRepository detalleRepo)
        {
            _repo = repo;
            _detalleRepo = detalleRepo;
        }

        public void MostrarTodo()
        {
            var compras = _repo.ObtenerTodos();

            foreach (var compra in compras)
            {
                var detalles = _detalleRepo.ObtenerTodos().Where(d => d.CompraId == compra.Id).ToList();
                double total = detalles.Sum(d => d.Valor * d.Cantidad);

                Console.WriteLine($"🧾 ID Compra: {compra.Id} | Fecha: {compra.Fecha:dd/MM/yyyy} | Total: ${total:F2}");

                foreach (var d in detalles)
                {
                    Console.WriteLine($"\t📦 Producto: {d.ProductoId}, Cantidad: {d.Cantidad}, Valor Unitario: ${d.Valor:F2}");
                }

                Console.WriteLine(new string('-', 60));
            }
        }

        public void CrearCompra(Compra compra)
        {
            _repo.Crear(compra);
        }

        public Compra? ObtenerCompraPorId(int id)
        {
            return _repo.ObtenerPorId(id);
        }

        public void EliminarCompra(int id)
        {
            // Primero eliminamos los detalles asociados a la compra
            var detalles = _detalleRepo.ObtenerTodos().Where(d => d.CompraId == id).ToList();
            foreach (var detalle in detalles)
            {
                _detalleRepo.Eliminar(detalle.Id);
            }

            // Luego eliminamos la compra principal
            _repo.Eliminar(id);
        }
        public void ActualizarCompra(Compra compraActualizada)
{
    // Eliminar detalles anteriores
    var detallesAnteriores = _detalleRepo.ObtenerTodos()
                                         .Where(d => d.CompraId == compraActualizada.Id)
                                         .ToList();

    foreach (var detalle in detallesAnteriores)
    {
        _detalleRepo.Eliminar(detalle.Id);
    }

    // Actualizar los datos de la compra
    _repo.Actualizar(compraActualizada);

    // Insertar los nuevos detalles con el ID correcto
    foreach (var nuevoDetalle in compraActualizada.Detalles)
    {
        nuevoDetalle.CompraId = compraActualizada.Id;
        _detalleRepo.Crear(nuevoDetalle);
    }
}

    }
}
