using System.ComponentModel.DataAnnotations;

namespace PrestigeFinancial.Shared.Models
{
    public class PagosDetalle
    {
        [Key]
        public int DetalleId { get; set; }
        public int PagoId { get; set; }
        public int PrestamoId { get; set; }
        public int Cantidadpagos { get; set; }
        public double ValorPagado { get; set; }
    }
}