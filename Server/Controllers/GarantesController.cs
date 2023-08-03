using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestigeFinancial.Server.DAL;
using PrestigeFinancial.Shared.Models;

namespace PrestigeFinancial.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GarantesController : ControllerBase
{
    private readonly Contexto _context;

    public GarantesController(Contexto context)
    {
        _context = context;
    }

    // GET: api/Garantes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Garantes>>> GetGarantes()
    {
      if (_context.Garantes == null)
      {
          return NotFound();
      }
        return await _context.Garantes.ToListAsync();
    }

    // GET: api/Garantes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Garantes>> GetGarantes(int id)
    {
      if (_context.Garantes == null)
      {
          return NotFound();
      }
        var Garantes = await _context.Garantes.FindAsync(id);

        if (Garantes == null)
        {
            return NotFound();
        }

        return Garantes;
    }

    // POST: api/Garantes
    [HttpPost]
    public async Task<ActionResult<Garantes>> PostGarantes(Garantes Garantes)
    {
        if (GarantesExists(Garantes.GaranteId))
            _context.Garantes.Update(Garantes);
        else
            _context.Garantes.Add(Garantes);

        await _context.SaveChangesAsync();

        return Ok(Garantes);
    }

    // DELETE: api/Garantes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGarantes(int id)
    {
        if (_context.Garantes == null)
        {
            return NotFound();
        }
        var Garantes = await _context.Garantes.FindAsync(id);
        if (Garantes == null)
        {
            return NotFound();
        }

        _context.Garantes.Remove(Garantes);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool GarantesExists(int id)
    {
        return (_context.Garantes?.Any(e => e.GaranteId == id)).GetValueOrDefault();
    }
}