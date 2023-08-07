using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrestigeFinancial.Shared.Models
{
    public class Pagos
    {
        [Key]
        public int PagoId { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public int PrestamoId { get; set; }
        
        [Required(ErrorMessage =("El concepto es requerido"))]
        public String? Concepto{get; set;}
        public int CantidadCoutasPagadas { get; set;}
        public double Monto { get; set; }

        [ForeignKey("PagoId")]
        public virtual List<PagosDetalle>? PagosDetalle {get;  set;} = new List<PagosDetalle>();

    }

}