using InventoryManagement.Domain.Entities;
using SistemaGestorV.Domain.Ports;
using System;
using System.Collections.Generic;

namespace SistemaGestorV.Application.Services
{
    public class EmpresaService
    {
        private readonly IEmpresaRepository _repo;

        public EmpresaService(IEmpresaRepository repo)
        {
            _repo = repo;
        }

        public EmpresaService()
        {
        }

        public void MostrarTodas()
        {
            var empresas = _repo.ObtenerTodos();
            Console.WriteLine("\n--- Lista de Empresas ---");
            foreach (var e in empresas)
            {
                Console.WriteLine($"ID: {e.id}, nombre: {e.nombre}, direccionId: {e.direccionId}, fechaReg: {e.fechaReg}");
            }
            Console.WriteLine(new string('-', 60));
        }

        public void CrearEmpresa(Empresa empresa)
        {
            _repo.Crear(empresa);
        }

        public bool ActualizarEmpresa(string id, int direccionId, string nombre, DateTime fechaReg)
        {
            var empresa = _repo.ObtenerPorId(id);

            if (empresa == null)
            {
                Console.WriteLine("❌ Empresa no encontrada.");
                return false;
            }

            empresa.direccionId = direccionId;
            empresa.nombre = nombre;

            _repo.Actualizar(empresa);
            return true;
        }
        public void EliminarEmpresa(string id)
        {
            var empresa = _repo.ObtenerPorId(id);

            if (empresa == null)
            {
                Console.WriteLine("❌ Empresa no encontrada.");
                return;
            }

            _repo.Eliminar(id);
            Console.WriteLine($"✅ Empresa ID: {id} eliminada con éxito.");
        }
        public IEnumerable<Empresa> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public Empresa ObtenerPorId(string id)
        {
            return _repo.ObtenerPorId(id);
        }
    }
}
