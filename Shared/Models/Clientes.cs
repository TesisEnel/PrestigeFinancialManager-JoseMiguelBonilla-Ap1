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
        [Required(ErrorMessage = "La cedula es requerido")]
        public string? cedula { get; set; }
        public DateTime FechaNacimiento { get; set; } 
        public string? Direccion { get; set; }

        [ForeignKey("ClienteId")]
        public ICollection<ClientesDetalle> ClientesDetalle { get; set; } = new List<ClientesDetalle>();

        [ForeignKey("ClienteId")]
        public ICollection<Prestamos> Prestamos { get; set; } = new List<Prestamos>();
    }

}