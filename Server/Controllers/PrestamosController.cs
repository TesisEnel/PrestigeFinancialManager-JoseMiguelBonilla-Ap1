using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestigeFinancial.Server.DAL;
using PrestigeFinancial.Shared.Models;

namespace PrestigeFinancial.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrestamosController : ControllerBase
{
    private readonly Contexto _context;

    public PrestamosController(Contexto context)
    {
        _context = context;
    }

     [HttpGet]
        public IEnumerable<Prestamos> Get()
        {
            return _context.Prestamos.AsNoTracking().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Prestamos> GetById(int id)
        {
            var prestamo = _context.Prestamos
                .Where(p => p.PrestamoId == id)
                .AsNoTracking()
                .SingleOrDefault();

            if (prestamo == null)
            {
                return NotFound();
            }

            return prestamo;
        }

        [HttpPost]
        public ActionResult<Prestamos> Create(Prestamos prestamo)
        {
            _context.Prestamos.Add(prestamo);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = prestamo.PrestamoId }, prestamo);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Prestamos prestamo)
        {
            if (id != prestamo.PrestamoId)
            {
                return BadRequest();
            }

            _context.Entry(prestamo).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var prestamo = _context.Prestamos.Find(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            _context.Prestamos.Remove(prestamo);
            _context.SaveChanges();

            return NoContent();
        }
}
