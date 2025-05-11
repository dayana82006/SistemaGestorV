namespace InventoryManagement.Domain.Entities
{
    public class Direccion
    {
        public int id { get; set; }
        public string ciudadId { get; set; } = string.Empty;
        public int calleNumero { get; set; }
        public int calleNombre { get; set; }
    }
}