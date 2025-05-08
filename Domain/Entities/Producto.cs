namespace SistemaGestorV.Domain.Entities;
using System;
using System.Collections.Generic;
public class Producto
{
   public string id { get; set; } = string.Empty;         
    public string nombre { get; set; } = string.Empty;      
    public int stock { get; set; }
    public int stockMin { get; set; }
    public int stockMax { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
    public string barcode { get; set; } = string.Empty;   
}
