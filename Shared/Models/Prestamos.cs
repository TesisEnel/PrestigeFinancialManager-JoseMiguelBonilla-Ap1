using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrestigeFinancial.Shared.Models
{
    public class Prestamos
    {
        [Key]
        public int PrestamoId { get; set; }
        public int ClienteId { get; set; }
        public int TiposPrestamoId {get; set; }
        public string? Nombres { get; set; }
        public int GaranteId { get; set; }
        public DateTime FechaPrestamo { get; set; } = DateTime.Today;

        [Required(ErrorMessage ="El monto es requerido")]
        public double MontoSolicitado { get; set; }

        [Required(ErrorMessage ="El interes es requerido")]
        public decimal Interes { get; set; }

        [Required(ErrorMessage ="Las coutas son requeridas")]
        public int Coutas { get; set; }
        public int CoutasOriginal { get; set; }
        public double MontoCoutas { get; set; }
        public double Balance { get; set; }
        public string? TipoPrestamo { get; set; }

    }
}