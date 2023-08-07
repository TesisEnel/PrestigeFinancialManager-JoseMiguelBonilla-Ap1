
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
        private readonly Contexto _context;

        public PagosController(Contexto contexto)
        {
            _context = contexto;
        }

                public bool Existe(int PagoId)
        {
            return (_context.Pagos?.Any(e => e.PagoId == PagoId)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pagos>>> Obtener()
        {
            if (_context.Pagos == null)
            {
                return NotFound();
            }
            else
            {
                return await _context.Pagos.ToListAsync();
            }
        }

        [HttpGet("{PagoId}")]
        public async Task<ActionResult<Pagos>> ObtenerPagos(int PagoId)
        {
            if (_context.Pagos == null)
            {
                return NotFound();
            }

            var Pagos = await _context.Pagos.Include(e => e.PagosDetalle ).Where(e => e.PagoId == PagoId).FirstOrDefaultAsync();

            if (Pagos == null)
            {
                return NotFound();
            }

            foreach (var item in Pagos.PagosDetalle )
            {
                Console.WriteLine($"{item.DetalleId}, {item.PagoId}, {item.PrestamoId}, {item.Cantidadpagos}");
            }

            return Pagos;
        }

        [HttpPost]
        public async Task<ActionResult<Pagos>> PostPagos(Pagos Pagos)
        {
            if (!Existe(Pagos.PagoId))
            {
                Prestamos? prestamo = new Prestamos();
                foreach (var prestamoConsumido in Pagos.PagosDetalle)
                {
                    prestamo = _context.Prestamos.Find(prestamoConsumido.PrestamoId);

                    if (prestamo != null)
                    {
                        prestamo.Coutas -= prestamoConsumido.Cantidadpagos;
                        _context.Prestamos.Update(prestamo);
                        await _context.SaveChangesAsync();
                        _context.Entry(prestamo).State = EntityState.Detached;
                    }
                }
                await _context.Pagos.AddAsync(Pagos);
            }
            else
            {
                var PagosAnterior = _context.Pagos.Include(e => e.PagosDetalle ).AsNoTracking()
                .FirstOrDefault(e => e.PagoId == Pagos.PagoId);

                Prestamos? prestamo = new Prestamos();

                if (PagosAnterior != null && PagosAnterior.PagosDetalle  != null)
                {
                    foreach (var prestamoConsumido in PagosAnterior.PagosDetalle )
                    {
                        if (prestamoConsumido != null)
                        {
                            prestamo = _context.Prestamos.Find(prestamoConsumido.PrestamoId);

                            if (prestamo != null)
                            {
                                prestamo.Coutas += prestamoConsumido.Cantidadpagos;
                                _context.Prestamos.Update(prestamo);
                                await _context.SaveChangesAsync();
                                _context.Entry(prestamo).State = EntityState.Detached;
                            }
                        }
                    }
                }

                if (PagosAnterior != null)
                {
                    prestamo = _context.Prestamos.Find(PagosAnterior.PagoId);

                    if (prestamo != null)
                    {
                        prestamo.Coutas -= PagosAnterior.CantidadCoutas;
                        _context.Prestamos.Update(prestamo);
                        await _context.SaveChangesAsync();
                        _context.Entry(prestamo).State = EntityState.Detached;
                    }
                }

                _context.Database.ExecuteSqlRaw($"Delete from PagosDetalle  where PagoId = {Pagos.PagoId}");

                foreach (var prestamoConsumido in Pagos.PagosDetalle )
                {
                    prestamo = _context.Prestamos.Find(prestamoConsumido.PrestamoId);

                    if (prestamo != null)
                    {
                        prestamo.Coutas -= prestamoConsumido.Cantidadpagos;
                        _context.Prestamos.Update(prestamo);
                        await _context.SaveChangesAsync();
                        _context.Entry(prestamo).State = EntityState.Detached;
                        _context.Entry(prestamoConsumido).State = EntityState.Added;
                    }
                }

                prestamo = _context.Prestamos.Find(Pagos.PagoId);

                if (prestamo != null)
                {
                    prestamo.Coutas += Pagos.CantidadCoutas;
                    _context.Prestamos.Update(prestamo);
                    await _context.SaveChangesAsync();
                    _context.Entry(prestamo).State = EntityState.Detached;
                }
                _context.Pagos.Update(Pagos);
            }

            await _context.SaveChangesAsync();
            _context.Entry(Pagos).State = EntityState.Detached;
            return Ok(Pagos);
        }

        [HttpDelete("{PagoId}")]
        public async Task<IActionResult> EliminarPagos(int PagoId)
        {
            var Pagos = await _context.Pagos.Include(e => e.PagosDetalle ).FirstOrDefaultAsync(e => e.PagoId == PagoId);

            if (Pagos == null)
            {
                return NotFound();
            }

            foreach (var prestamoConsumido in Pagos.PagosDetalle )
            {
                var prestamo = await _context.Prestamos.FindAsync(prestamoConsumido.PrestamoId);

                if (prestamo != null)
                {
                    prestamo.Coutas += prestamoConsumido.Cantidadpagos;
                    _context.Prestamos.Update(prestamo);
                }
            }

            var prestamoInicial = await _context.Prestamos.FindAsync(Pagos.PagoId);

            if (prestamoInicial != null)
            {
                prestamoInicial.Coutas += Pagos.CantidadCoutas;
                _context.Prestamos.Update(prestamoInicial);
            }

            _context.Pagos.Remove(Pagos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

