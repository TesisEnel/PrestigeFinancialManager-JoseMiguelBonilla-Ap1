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

    // GET: api/Prestamos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Prestamos>>> GetPrestamos()
    {
      if (_context.Prestamos == null)
      {
          return NotFound();
      }
        return await _context.Prestamos.ToListAsync();
    }

    // GET: api/Prestamos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Prestamos>> GetPrestamos(int id)
    {
      if (_context.Prestamos == null)
      {
          return NotFound();
      }
        var Prestamos = await _context.Prestamos.FindAsync(id);

        if (Prestamos == null)
        {
            return NotFound();
        }

        return Prestamos;
    }

    // POST: api/Prestamos
    [HttpPost]
    public async Task<ActionResult<Prestamos>> PostPrestamos(Prestamos Prestamos)
    {
        if (PrestamosExists(Prestamos.PrestamoId))
            _context.Prestamos.Update(Prestamos);
        else
            _context.Prestamos.Add(Prestamos);

        await _context.SaveChangesAsync();

        return Ok(Prestamos);
    }

    // DELETE: api/Prestamos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrestamos(int id)
    {
        if (_context.Prestamos == null)
        {
            return NotFound();
        }
        var Prestamos = await _context.Prestamos.FindAsync(id);
        if (Prestamos == null)
        {
            return NotFound();
        }

        _context.Prestamos.Remove(Prestamos);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PrestamosExists(int id)
    {
        return (_context.Prestamos?.Any(e => e.PrestamoId == id)).GetValueOrDefault();
    }
}