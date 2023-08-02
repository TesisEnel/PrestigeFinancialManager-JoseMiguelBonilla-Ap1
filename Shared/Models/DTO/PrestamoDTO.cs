using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrestigeFinancial.Shared.Models.DTO
{
    public class PrestamoDTO
    {
        public int ClienteId { get; set; }
        public String? Nombres { get; set; }

        //DATOS DEL PRESTAMO 
        public int PrestamoId { get; set; }
        public double MontoSolicitado { get; set; }
        public decimal Interes { get; set; }
        public int Coutas { get; set; }
        
        
    }

}