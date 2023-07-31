using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestigeFinancial.Shared;
using PrestigeFinancial.Server.DAL;
using PrestigeFinancial.Shared.Models;

namespace PrestigeFinancial.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TiposTelefonosController : ControllerBase
{
    private readonly Contexto _context;

    public TiposTelefonosController(Contexto context)
    {
        _context = context;
    }

    // GET: api/TiposTelefonos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TiposTelefonos>> GetTiposTelefonos(int id)
    {
        var tipo = await _context.TiposTelefonos.FindAsync(id);

        return tipo!;
    }

    // GET: api/TiposTelefonos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TiposTelefonos>>> GetTiposTelefonos()
    {
        if (_context.TiposTelefonos == null)
        {
            return NotFound();
        }
        return await _context.TiposTelefonos.ToListAsync();
    }
}