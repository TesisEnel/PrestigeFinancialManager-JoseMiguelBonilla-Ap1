using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrestigeFinancial.Shared.Models
{
    public class Garantes
    {
        [Key]
        public int GaranteId { get; set; }
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string? Nombres { get; set; }
        [Required(ErrorMessage = "La cedula es requerido")]
        public string? cedula { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }

    }

}