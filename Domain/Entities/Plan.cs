using System;
using System.Collections.Generic;

namespace SistemaGestorV.Domain.Entities
{
    public class Plan
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public double Descuento { get; set; }
        public List<string> ProductosAsociados { get; set; } = new List<string>();

        public bool EsValido()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
                return false;
            
            if (Descuento <= 0 || Descuento > 100)
                return false;
                
            if (FechaInicio >= FechaFin)
                return false;
                
            return true;
        }
    }
}