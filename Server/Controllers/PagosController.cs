
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

            var Pagos = await _context.Pagos.Include(e => e.PagosDetalle).Where(e => e.PagoId == PagoId).FirstOrDefaultAsync();

            if (Pagos == null)
            {
                return NotFound();
            }

            foreach (var item in Pagos.PagosDetalle)
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
                Prestamos? prestamos = new Prestamos();
                foreach (var prestamosConsumido in Pagos.PagosDetalle)
                {
                    prestamos = _context.Prestamos.Find(prestamosConsumido.PrestamoId);

                    if (prestamos != null)
                    {
                        prestamos.Coutas -= prestamosConsumido.Cantidadpagos;
                        _context.Prestamos.Update(prestamos);
                        await _context.SaveChangesAsync();
                        _context.Entry(prestamos).State = EntityState.Detached;
                    }
                }
                await _context.Pagos.AddAsync(Pagos);
            }
            else
            {
                var PagosAnterior = _context.Pagos.Include(e => e.PagosDetalle).AsNoTracking()
                .FirstOrDefault(e => e.PagoId == Pagos.PagoId);

                Prestamos? prestamos = new Prestamos();

                if (PagosAnterior != null && PagosAnterior.PagosDetalle != null)
                {
                    foreach (var prestamosConsumido in PagosAnterior.PagosDetalle)
                    {
                        if (prestamosConsumido != null)
                        {
                            prestamos = _context.Prestamos.Find(prestamosConsumido.PrestamoId);

                            if (prestamos != null)
                            {
                                prestamos.Coutas += prestamosConsumido.Cantidadpagos;
                                _context.Prestamos.Update(prestamos);
                                await _context.SaveChangesAsync();
                                _context.Entry(prestamos).State = EntityState.Detached;
                            }
                        }
                    }
                }

                if (PagosAnterior != null)
                {
                    prestamos = _context.Prestamos.Find(PagosAnterior.PrestamoId);

                    if (prestamos != null)
                    {
                        prestamos.Coutas -= PagosAnterior.CantidadCoutasPagadas;
                        _context.Prestamos.Update(prestamos);
                        await _context.SaveChangesAsync();
                        _context.Entry(prestamos).State = EntityState.Detached;
                    }
                }

                _context.Database.ExecuteSqlRaw($"Delete from PagosDetalle where PagoId = {Pagos.PagoId}");

                foreach (var prestamosConsumido in Pagos.PagosDetalle)
                {
                    prestamos = _context.Prestamos.Find(prestamosConsumido.PrestamoId);

                    if (prestamos != null)
                    {
                        prestamos.Coutas -= prestamosConsumido.Cantidadpagos;
                        _context.Prestamos.Update(prestamos);
                        await _context.SaveChangesAsync();
                        _context.Entry(prestamos).State = EntityState.Detached;
                        _context.Entry(prestamosConsumido).State = EntityState.Added;
                    }
                }

                prestamos = _context.Prestamos.Find(Pagos.PrestamoId);

                if (prestamos != null)
                {
                    prestamos.Coutas += Pagos.CantidadCoutasPagadas;
                    _context.Prestamos.Update(prestamos);
                    await _context.SaveChangesAsync();
                    _context.Entry(prestamos).State = EntityState.Detached;
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
            var Pagos = await _context.Pagos.Include(e => e.PagosDetalle).FirstOrDefaultAsync(e => e.PagoId == PagoId);

            if (Pagos == null)
            {
                return NotFound();
            }

            foreach (var prestamosConsumido in Pagos.PagosDetalle)
            {
                var prestamos = await _context.Prestamos.FindAsync(prestamosConsumido.PrestamoId);

                if (prestamos != null)
                {
                    prestamos.Coutas += prestamosConsumido.Cantidadpagos;
                    _context.Prestamos.Update(prestamos);
                }
            }

            var prestamosInicial = await _context.Prestamos.FindAsync(Pagos.PrestamoId);

            if (prestamosInicial != null)
            {
                prestamosInicial.Coutas += Pagos.CantidadCoutasPagadas;
                _context.Prestamos.Update(prestamosInicial);
            }

            _context.Pagos.Remove(Pagos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

}