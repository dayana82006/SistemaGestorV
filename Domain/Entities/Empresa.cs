namespace InventoryManagement.Domain.Entities
{
    public class Empresa
    {
        public int id { get; set; }
        public string nombre { get; set; } = string.Empty;
        public int direccionId { get; set; }
        public int fechaReg { get; set; }
    }
}