using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrestigeFinance.Shared.Models
{
    public class TiposPrestamos
    {
        [Key]
        public int TiposPrestamoId {get; set; }
        
        [Required(ErrorMessage = "La Descripcion es requerida")]
        public string? DescripcionPrestamo {get; set; }

    }
}