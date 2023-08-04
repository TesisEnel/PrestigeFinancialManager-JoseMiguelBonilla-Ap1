using System.ComponentModel.DataAnnotations;

namespace PrestigeFinancial.Shared.Models
{
    public class PagosDetalle
    {
        [Key]
        public int DetalleId { get; set; }

        public int? PagoId { get; set; }

        public int PrestamoId { get; set; }

        [Required(ErrorMessage = "El Valor Pagado es requerido")]  
        [Range(0.01, double.MaxValue, ErrorMessage = "El Valor Pagado  debe ser mayor  que cero")]
        public double ValorPagado { get; set; }
    }
}