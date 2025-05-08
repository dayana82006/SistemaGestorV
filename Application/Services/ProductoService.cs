using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System;
namespace SistemaGestorV.Application.Services
{
    public class ProductoService
    {
        private readonly IProductoRepository _repo;

        public ProductoService(IProductoRepository repo)
        {
            _repo = repo;
        }
        public void MostrarTodos()
        {
            var productos = _repo.ObtenerTodos();
            Console.WriteLine("\n--- Lista de Productos ---");
            foreach (var producto in productos)
            {
                Console.WriteLine($"ID: {producto.Id}, Nombre: {producto.Nombre}, Stock: {producto.Stock}");
            }
        }

        // Crea un nuevo producto
        public void CrearProducto(Producto producto)
        {
            _repo.Crear(producto);
        }

        // Actualiza un producto existente
        public void ActualizarProducto(string id, string nombre, int stock)
        {
            var producto = _repo.ObtenerPorId(id);

            if (producto == null)
            {
                Console.WriteLine("Producto no encontrado.");
                return;
            }

            if (string.IsNullOrWhiteSpace(nombre) || stock < 0)
            {
                Console.WriteLine("Datos inválidos para actualizar el producto.");
                return;
            }

            producto.Nombre = nombre.Trim();
            producto.Stock = stock;

            _repo.Actualizar(producto);
            Console.WriteLine($"Producto ID: {id} actualizado con éxito.");
        }

        // Elimina un producto por ID
        public void EliminarProducto(string id)
        {
            var producto = _repo.ObtenerPorId(id);

            if (producto == null)
            {
                Console.WriteLine("Producto no encontrado.");
                return;
            }

            _repo.Eliminar(id);
            Console.WriteLine($"Producto ID: {id} eliminado con éxito.");
        }
    }
}
