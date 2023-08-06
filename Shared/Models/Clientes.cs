using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrestigeFinancial.Shared.Models
{
    public class Clientes
    {
        [Key]
        public int ClienteId { get; set; }
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string? Nombres { get; set; }
        [StringLength(11, MinimumLength = 11, ErrorMessage = "El campo cedula debe tener exactamente 11 caracteres.")]
        public string? cedula { get; set; }
        public DateTime FechaNacimiento { get; set; } 
        public string? Direccion { get; set; }
        public double Balance { get; set; }

        [ForeignKey("ClienteId")]
        public ICollection<ClientesDetalle> ClientesDetalle { get; set; } = new List<ClientesDetalle>();

    }

}