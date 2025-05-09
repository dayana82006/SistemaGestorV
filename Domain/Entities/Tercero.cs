using System.Collections.Generic;

namespace SistemaGestorV.Domain.Entities
{
    public class Tercero
    {
        public Tercero()
        {
            Telefonos = new List<Telefono>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TipoDocumentoId { get; set; }
        public int TipoTerceroId { get; set; }
        public int DireccionId { get; set; }
        public string EmpresaId { get; set; } = string.Empty;
        public List<Telefono> Telefonos { get; set; }
        
        // Propiedades de navegación (nullable)
        public Cliente? Cliente { get; set; }
        public Empleado? Empleado { get; set; }
        public Proveedor? Proveedor { get; set; }

        public string TipoTerceroDescripcion => TipoTerceroId switch
        {
            1 => "Cliente",
            2 => "Empleado",
            3 => "Proveedor",
            _ => "Desconocido"
        };
    }
}