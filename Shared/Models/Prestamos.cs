using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrestigeFinancial.Shared.Models
{
    public class Prestamos
    {
        [Key]

        public int PrestamoId { get; set; }
        public int ClienteId { get; set; }

        [Required(ErrorMessage ="El nombre del deudor es requerido")]
        public string? Deudor { get; set; }

        [Required(ErrorMessage ="La Cedula del deudor es requerido")]
        public string? Cedula { get; set; }
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;

        [Required(ErrorMessage ="El monto es requerido")]
        public double MontoSolicitado { get; set; }
        public double MontoTotal { get; set; }

        [Required(ErrorMessage ="El interes es requerido")]
        public decimal Interes { get; set; }

        [Required(ErrorMessage ="Las coutas son requeridas")]
        public int Coutas { get; set; }
        public double MontoCoutas { get; set; }
        public double Balance { get; set; }
        public double Restante { get; set; }


    }
}