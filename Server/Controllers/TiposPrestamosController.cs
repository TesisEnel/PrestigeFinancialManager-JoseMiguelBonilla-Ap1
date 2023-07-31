using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestigeFinancial.Shared;
using PrestigeFinancial.Server.DAL;
using PrestigeFinancial.Shared.Models;

namespace PrestigeFinancial.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TiposPrestamosController : ControllerBase
{
    private readonly Contexto _context;

    public TiposPrestamosController(Contexto context)
    {
        _context = context;
    }

    // GET: api/TiposPrestamos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TiposPrestamos>> GetTipoPrestamos(int id)
    {
        var tipo = await _context.TiposPrestamos.FindAsync(id);

        return tipo!;
    }

    // GET: api/TiposPrestamos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TiposPrestamos>>> GetTiposPrestamos()
    {
        if (_context.TiposPrestamos == null)
        {
            return NotFound();
        }
        return await _context.TiposPrestamos.ToListAsync();
    }
}