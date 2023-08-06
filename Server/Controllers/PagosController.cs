
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestigeFinancial.Server.DAL;
using PrestigeFinancial.Shared.Models;

namespace PrestigeFinancial.Server.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly Contexto _contexto;

        public PagosController(Contexto contexto)
        {
            _contexto = contexto;
        }

        [HttpGet("{pagosId}")]
        public ActionResult<bool> Existe(int pagosId)
        {
            return _contexto.Pagos.Any(o => o.PagoId == pagosId);
        }

        [HttpPost]
        public ActionResult<bool> Guardar(Pagos pago)
        {
            if (!Existe(pago.PagoId).Value)
            {
                return Insertar(pago);
            }
            else
            {
                return Modificar(pago);
            }
        }

        [HttpDelete("{pagosId}")]
        public ActionResult<bool> Eliminar(int pagosId)
        {
            var pago = _contexto.Pagos.Include(p => p.PagosDetalle).FirstOrDefault(p => p.PagoId == pagosId);
            if (pago == null)
            {
                return NotFound();
            }

            var cliente = _contexto.Clientes.Find(pago.ClienteId);

            if (pago.PagosDetalle != null)
            {
                foreach (var detalle in pago.PagosDetalle)
                {
                    var prestamo = _contexto.Prestamos.Find(detalle.PrestamoId);
                    if (prestamo != null)
                    {
                        prestamo.Balance += detalle.ValorPagado;
                        _contexto.Entry(prestamo).State = EntityState.Modified;
                    }

                    if (cliente != null)
                    {
                        cliente.Balance += detalle.ValorPagado;
                        _contexto.Entry(cliente).State = EntityState.Modified;
                    }
                }
            }

            var pagosDetalleAEliminar = _contexto.Set<PagosDetalle>().Where(pd => pd.PagoId == pago.PagoId);
            _contexto.Set<PagosDetalle>().RemoveRange(pagosDetalleAEliminar);
            _contexto.Pagos.Remove(pago);
            _contexto.SaveChanges();

            return true;
        }

        [HttpGet("{pagosId}")]
        public ActionResult<Pagos> Buscar(int pagosId)
        {
            var pago = _contexto.Pagos.Include(o => o.PagosDetalle).FirstOrDefault(o => o.PagoId == pagosId);
            if (pago == null)
            {
                return NotFound();
            }

            return pago;
        }

        [HttpGet]
        public ActionResult<List<Pagos>> GetList(Expression<Func<Pagos, bool>> criterio)
        {
            var pagos = _contexto.Pagos.AsNoTracking().Where(criterio).ToList();
            return pagos;
        }

        private ActionResult<bool> Insertar(Pagos pago)
        {
            var cliente = _contexto.Clientes.Find(pago.ClienteId);

            if (pago.PagosDetalle != null)
            {
                foreach (var detalle in pago.PagosDetalle)
                {
                    var prestamo = _contexto.Prestamos.Find(detalle.PrestamoId);

                    if (prestamo != null)
                    {
                        prestamo.Balance -= detalle.ValorPagado;
                        _contexto.Entry(prestamo).State = EntityState.Modified;
                    }

                    if (cliente != null)
                    {
                        cliente.Balance -= detalle.ValorPagado;
                        _contexto.Entry(cliente).State = EntityState.Modified;
                    }
                }
            }

            _contexto.Pagos.Add(pago);
            _contexto.SaveChanges();

            return true;
        }

        private ActionResult<bool> Modificar(Pagos pago)
        {
            var pagoAnterior = _contexto.Pagos
                .Where(p => p.PagoId == pago.PagoId)
                .Include(p => p.PagosDetalle)
                .AsNoTracking()
                .SingleOrDefault();

            var cliente = _contexto.Clientes.Find(pago.ClienteId);

            if (pagoAnterior != null && pagoAnterior.PagosDetalle != null)
            {
                foreach (var detalle in pagoAnterior.PagosDetalle)
                {
                    var prestamo = _contexto.Prestamos.Find(detalle.PrestamoId);
                    if (prestamo != null)
                    {
                        prestamo.Balance += detalle.ValorPagado;
                        _contexto.Entry(prestamo).State = EntityState.Modified;
                    }

                    if (cliente != null)
                    {
                        cliente.Balance += detalle.ValorPagado;
                        _contexto.Entry(cliente).State = EntityState.Modified;
                    }
                }
            }

            if (pago.PagosDetalle != null)
            {
                foreach (var detalle in pago.PagosDetalle)
                {
                    var prestamo = _contexto.Prestamos.Find(detalle.PrestamoId);
                    if (prestamo != null)
                    {
                        prestamo.Balance -= detalle.ValorPagado;
                        _contexto.Entry(prestamo).State = EntityState.Modified;
                    }

                    if (cliente != null)
                    {
                        cliente.Balance -= detalle.ValorPagado;
                        _contexto.Entry(cliente).State = EntityState.Modified;
                    }
                }
            }

            var pagosDetalleAEliminar = _contexto.Set<PagosDetalle>().Where(pd => pd.PagoId == pago.PagoId);
            _contexto.Set<PagosDetalle>().RemoveRange(pagosDetalleAEliminar);
            _contexto.Entry(pago).State = EntityState.Modified;
            _contexto.SaveChanges();

            return true;
        }
    }

}

