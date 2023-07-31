using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestigeFinancial.Server.DAL;
using PrestigeFinancial.Shared.Models;

namespace ClienteApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly Contexto _context;

    public ClientesController(Contexto context)
    {
        _context = context;
    }

    // GET: api/Clientes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Clientes>>> GetClientes()
    {
        if (_context.Clientes == null)
        {
            return NotFound();
        }
        return await _context.Clientes.ToListAsync();
    }

    // GET: api/Clientes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Clientes>> GetCliente(int id)
    {
        if (_context.Clientes == null)
        {
            return NotFound();
        }
        var Clientes = await _context.Clientes
            .Include(c => c.ClientesDetalle)
            .Where(c => c.ClienteId == id)
            .FirstOrDefaultAsync();

        if (Clientes == null)
        {
            return NotFound();
        }

        return Clientes;
    }

    // POST: api/Clientes
    [HttpPost]
    public async Task<ActionResult<Clientes>> PostCliente(Clientes cliente)
    {
        if (!ClientesExists(cliente.ClienteId))
            _context.Clientes.Add(cliente);
        else
            _context.Clientes.Update(cliente);

        await _context.SaveChangesAsync();

        return Ok(cliente);
    }

    // DELETE: api/Clientes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClientes(int id)
    {
        if (_context.Clientes == null)
        {
            return NotFound();
        }
        var Clientes = await _context.Clientes.FindAsync(id);
        if (Clientes == null)
        {
            return NotFound();
        }

        _context.Clientes.Remove(Clientes);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ClientesExists(int id)
    {
        return (_context.Clientes?.Any(e => e.ClienteId == id)).GetValueOrDefault();
    }
}