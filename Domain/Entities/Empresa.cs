namespace InventoryManagement.Domain.Entities
{
    public class Empresa
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public int direccionId { get; set; }
        public DateTime fechaReg { get; set; }
    }
}