using System.ComponentModel.DataAnnotations;

namespace PrestigeFinancial.Shared.Models
{
    public class ClientesDetalle
    {
        [Key]
        public int DetalleId { get; set; }
        public int ClienteId { get; set; }
        public int TiposTelefonoId { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "El campo Telefono debe tener exactamente 10 caracteres.")]
        public string? Telefono { get; set; }
    }
}
