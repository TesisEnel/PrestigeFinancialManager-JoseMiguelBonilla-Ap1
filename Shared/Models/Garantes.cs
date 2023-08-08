using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrestigeFinancial.Shared.Models
{
    public class Garantes
    {
        [Key]
        public int GaranteId { get; set; }
        [Required(ErrorMessage = "El Nombre es requerido")]
        public int ClienteId { get; set; }
        public string? Nombres { get; set; }
        [Required(ErrorMessage = "Cedula es requerido")]

        [StringLength(11, MinimumLength = 11, ErrorMessage = "El campo cedula debe tener exactamente 11 caracteres.")]
        public string? cedula { get; set; }
        [Required(ErrorMessage = "telefono es requerido")]


        [StringLength(10, MinimumLength = 10, ErrorMessage = "El campo Telefono debe tener exactamente 10 caracteres.")]
        public string? Telefono { get; set; }
        [Required(ErrorMessage = "direccion es requerido")]

        public string? Direccion { get; set; }

    }

}