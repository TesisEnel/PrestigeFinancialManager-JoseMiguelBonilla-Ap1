using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrestigeFinancial.Shared.Models
{
    public class Prestamos
    {
        [Key]
        public int PrestamoId { get; set; }

        [Required(ErrorMessage ="EL Cliente es requerido")]
        public int ClienteId { get; set; }

        public DateTime FechaPrestamo { get; set; } = DateTime.Now;

        [Required(ErrorMessage ="El monto es requerido")]
        public double MontoSolicitado { get; set; }

        [Required(ErrorMessage ="El interes es requerido")]
        public decimal Interes { get; set; }

        [Required(ErrorMessage ="Las coutas son requeridas")]
        public int Coutas { get; set; }
        public double Balance { get; set; }
        public string? TipoPrestamo { get; set; }

    }
}