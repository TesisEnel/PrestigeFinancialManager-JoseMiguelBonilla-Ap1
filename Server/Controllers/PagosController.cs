using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestigeFinancial.Server.DAL;
using PrestigeFinancial.Shared.Models;

namespace PrestigeFinancial.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PagosController : ControllerBase
{
        private readonly Contexto _context;

        public PagosController(Contexto context)
        {
            _context = context;
        }
        public bool Existe(int PagoId)
        {
            return (_context.Pagos?.Any(e => e.PagoId == PagoId)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pagos>>> Obtener()
        {
            if(_context.Pagos == null)
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
            if(_context.Pagos == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.Include(e => e.PagosDetalle).Where( e => e.PagoId == PagoId).FirstOrDefaultAsync();

            if(pago == null)
            {
                return NotFound();
            }

            foreach(var item in pago.PagosDetalle)
            {
                Console.WriteLine($"{item.DetalleId}, {item.PagoId}, {item.PrestamoId}, {item.ValorPagado}");
            }

            return pago;
        }
        
        [HttpPost]
        public async Task<ActionResult<Pagos>> PostPagos(Pagos Pagos)
        {
            if(!Existe(Pagos.PagoId))
            {
                Pagos? pagos = new Pagos();
                foreach(var pagosConsumido in Pagos.PagosDetalle)
                {
                    pagos = _context.Pagos.Find(pagosConsumido.PagoId);

                    if(pagos != null)
                    {
                        pagos.Monto -= pagosConsumido.ValorPagado;
                        _context.Pagos.Update(pagos);
                        await _context.SaveChangesAsync();
                        _context.Entry(pagos).State = EntityState.Detached;
                    }
                }
                await _context.Pagos.AddAsync(Pagos);
            }
            else
            {
                var pagoAnterior = _context.Pagos.Include(e => e.PagosDetalle).AsNoTracking()
                .FirstOrDefault(e => e.PagoId == Pagos.PagoId);

                Pagos? pagos = new Pagos();

                if(pagoAnterior != null && pagoAnterior.PagosDetalle != null)
                {
                    foreach(var pagosConsumido in pagoAnterior.PagosDetalle)
                    {
                        if(pagosConsumido != null)
                        {
                            pagos = _context.Pagos.Find(pagosConsumido.PagoId);

                            if(pagos != null)
                            {
                                pagos.Monto +=  pagosConsumido.ValorPagado;
                                _context.Pagos.Update(pagos);
                                await _context.SaveChangesAsync();
                                _context.Entry(pagos).State = EntityState.Detached;
                            }   
                        }
                    }
                }

                if(pagoAnterior != null)
                {
                    pagos = _context.Pagos.Find(pagoAnterior.PagoId);

                    if(pagos != null)
                    {
                        pagos.Monto -= pagoAnterior.Monto;
                        _context.Pagos.Update(pagos);
                        await _context.SaveChangesAsync();
                        _context.Entry(pagos).State = EntityState.Detached;
                    }
                }

                _context.Database.ExecuteSqlRaw($"Delete from PagosDetalle where PagoId = {Pagos.PagoId}");

                foreach(var pagosConsumido in Pagos.PagosDetalle)
                {
                    pagos = _context.Pagos.Find(pagosConsumido.PagoId);

                    if(pagos != null)
                    {
                        pagos.Monto -= pagosConsumido.ValorPagado;
                        _context.Pagos.Update(pagos);
                        await _context.SaveChangesAsync();
                        _context.Entry(pagos).State = EntityState.Detached;
                        _context.Entry(pagosConsumido).State = EntityState.Added;
                    }
                }

                pagos = _context.Pagos.Find(Pagos.PagoId);

                if(pagos != null)
                {
                    pagos.Monto += Pagos.Monto;
                    _context.Pagos.Update(pagos);
                    await _context.SaveChangesAsync();
                    _context.Entry(pagos).State = EntityState.Detached;
                }
                _context.Pagos.Update(Pagos);
            }

            await _context.SaveChangesAsync();
            _context.Entry(Pagos).State = EntityState.Detached;
            return Ok(Pagos);
        }

        [HttpDelete("{PagoId}")]
        public async Task<IActionResult> Eliminarpago(int PagoId)
        {
            var pago = await _context.Pagos.Include(e => e.PagosDetalle).FirstOrDefaultAsync(e => e.PagoId == PagoId);

            if (pago == null)
            {
                return NotFound();
            }

            foreach (var pagosConsumido in pago.PagosDetalle)
            {
                var pagos = await _context.Pagos.FindAsync(pagosConsumido.PagoId);

                if (pagos != null)
                {
                    pagos.Monto += pagosConsumido.ValorPagado;
                    _context.Pagos.Update(pagos);
                }
            }

            var pagosInicial = await _context.Pagos.FindAsync(pago.PagoId);

            if (pagosInicial != null)
            {
                pagosInicial.Monto += pago.Monto;
                _context.Pagos.Update(pagosInicial);
            }

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();

            return NoContent();
        }
}