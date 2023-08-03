using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrestigeFinancial.Shared.Models
{
    public class Pagos
    {
        [Key]
        public int PagoId { get; set; }

        [Required(ErrorMessage = "La Fecha es requerida")]  
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La Persona es requerida")]  
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El concepto es requerido")]  
        public String? Concepto{get; set;}

        [Required(ErrorMessage = "El monto es requerido")]  
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero")]
        public double Monto { get; set; }

        [ForeignKey("PagoId")]
        public virtual List<PagosDetalle>? PagosDetalle {get;  set;} = new List<PagosDetalle>();

    }

}