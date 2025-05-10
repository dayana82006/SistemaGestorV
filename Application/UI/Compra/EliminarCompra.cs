namespace SistemaGestorV.Application.UI.Compra
{
    using System;
    using SistemaGestorV.Application.Services;

    public class EliminarCompra
    {
        private readonly CompraService _servicio;

        public EliminarCompra(CompraService servicio)
        {
            _servicio = servicio;
        }

        public void Ejecutar()
        {
            Console.Write("🧾 Ingrese el ID de la compra que desea eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ ID inválido.");
                return;
            }

            var compra = _servicio.ObtenerCompraPorId(id);
            if (compra == null)
            {
                Console.WriteLine("⚠️ No se encontró una compra con ese ID.");
                return;
            }

            Console.WriteLine($"\n🔎 Se encontró la siguiente compra:");
            Console.WriteLine($"🆔 ID: {compra.Id}");
            Console.WriteLine($"📄 Documento: {compra.DocCompra}");
            Console.WriteLine($"📅 Fecha: {compra.Fecha.ToShortDateString()}");
            Console.WriteLine($"👤 Proveedor ID: {compra.TerceroProvId}, Empleado ID: {compra.TerceroEmpId}");
            Console.WriteLine($"📦 Productos en la compra: {compra.Detalles.Count}");

            Console.Write("\n❓ ¿Estás seguro de que deseas eliminar esta compra? (s/n): ");
            var confirmacion = Console.ReadLine()?.Trim().ToLower();

            if (confirmacion == "s")
            {
                try
                {
                    _servicio.EliminarCompra(id);
                    Console.WriteLine("✅ Compra eliminada exitosamente.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"❌ No se puede eliminar la compra: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error al eliminar la compra: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("🚫 Operación cancelada.");
            }
        }
    }
}
