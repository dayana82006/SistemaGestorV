namespace InventoryManagement.Domain.Entities
{
    public class Direccion
    {
        public int id { get; set; }
        public int ciudadId { get; set; }
        public string calleNumero { get; set; }
        public string calleNombre { get; set; }
    }
}