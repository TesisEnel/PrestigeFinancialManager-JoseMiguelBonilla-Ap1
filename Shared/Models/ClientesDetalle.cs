using System.ComponentModel.DataAnnotations;

namespace PrestigeFinancial.Shared.Models
{
    public class ClientesDetalle
    {
        [Key]
        public int DetalleId { get; set; }
        public int ClienteId { get; set; }
        public int TiposTelefonoId { get; set; }
        public string? Telefono { get; set; }
    }
}
