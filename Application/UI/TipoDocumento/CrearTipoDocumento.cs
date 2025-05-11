using System;
using System.Linq;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Application.Services;

namespace SistemaGestorV.Application.UI.TipoDocumento
{
    public class CrearTipoDocumento
    {
        private readonly TipoDocumentoService _servicio;

        public CrearTipoDocumento(TipoDocumentoService servicio)
        {
            _servicio = servicio;
        }

        public void Ejecutar()
        {
            var tipo_documento = new SistemaGestorV.Domain.Entities.Tipo_documento();

            Console.Write("Id: ");
            string id = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!int.TryParse(id, out int idInt))
            {
                Console.WriteLine("❌ El ID debe ser un número entero válido.");
                return;
            }

            var tipos_documento = _servicio.ObtenerTodos();

            if (tipos_documento.Any(td => td.id == idInt))
            {
                Console.WriteLine("❌ Ya existe un Tipo de documento con ese ID.");
                return;
            }

            Console.Write("Descripción: ");
            string descripcion = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(descripcion))
            {
                Console.WriteLine("❌ La descripción no puede estar vacío.");
                return;
            }

            if (tipos_documento.Any(td => td.descripcion.Equals(descripcion, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("❌ Ya existe un tipo de documento con esa descripcion.");
                return;
            }

            tipo_documento.id = idInt;
            tipo_documento.descripcion = descripcion;

            _servicio.CrearTipoDocumento(tipo_documento);
            Console.WriteLine("✅ Tipo de documento creado con éxito.");
        }
    }
}
