using InventoryManagement.Domain.Entities;
using SistemaGestorV.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System;
using System.Collections.Generic;

namespace SistemaGestorV.Application.Services
{
    public class TipoDocumentoService
    {
        private readonly ITipoDocumentoRepository _repo;

        public TipoDocumentoService(ITipoDocumentoRepository repo)
        {
            _repo = repo;
        }

        public TipoDocumentoService()
        {
        }

        public void MostrarTodos()
        {
            var Tipo_documento = _repo.ObtenerTodos();
            Console.WriteLine("\n--- Lista de tipos de documento ---");
            foreach (var td in Tipo_documento)
            {
                Console.WriteLine($"ID: {td.id}, descripcion: {td.descripcion}");
            }
            Console.WriteLine(new string('-', 60));
        }

        public void CrearTipoDocumento(Tipo_documento tipo_documento)
        {
            _repo.Crear(tipo_documento);
        }

                public bool ActualizarTipoDocumento(string id, string descripcion)
        {
            var tipo_documento = _repo.ObtenerPorId(id);

            if (tipo_documento == null)
            {
                Console.WriteLine("❌ Tipo de documento no encontrada.");
                return false;
            }

            tipo_documento.descripcion = descripcion.Trim();
            _repo.Actualizar(tipo_documento);

            return true;
        }


        public void EliminarTipoDocumento(string id)
        {
            var tipo_documento = _repo.ObtenerPorId(id);

            if (tipo_documento == null)
            {
                Console.WriteLine("Tipo de documento no encontrada.");
                return;
            }

            _repo.Eliminar(id);
            Console.WriteLine($"Tipo de documento ID: {id} eliminado con éxito.");
        }

        public IEnumerable<Tipo_documento> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public Tipo_documento ObtenerPorId(string id)
        {
            return _repo.ObtenerPorId(id);
        }
    }
}