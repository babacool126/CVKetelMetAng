using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CVKetelMetAng.Data;
using CVKetelMetAng.Models;
using Microsoft.Extensions.Logging;

namespace CVKetelMetAng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AfspraaksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AfspraaksController> _logger;

        public AfspraaksController(ApplicationDbContext context, ILogger<AfspraaksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Afspraaks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Afspraak>>> GetAfspraken()
        {
            return await _context.Afspraken.Include(a => a.Klant).ToListAsync();
        }

        // GET: api/Afspraaks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Afspraak>> GetAfspraak(int id)
        {
            var afspraak = await _context.Afspraken.Include(a => a.Klant).FirstOrDefaultAsync(a => a.Id == id);

            if (afspraak == null)
            {
                return NotFound();
            }

            return afspraak;
        }

        // PUT: api/Afspraaks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAfspraak(int id, Afspraak afspraak)
        {
            if (id != afspraak.Id)
            {
                return BadRequest();
            }

            _context.Entry(afspraak).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AfspraakExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Afspraaks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Afspraak>> PostAfspraak([FromBody] Afspraak afspraak)
        {
            if (afspraak.KlantId > 0 && afspraak.Klant != null)
            {
                // If KlantId is provided, ignore the Klant object to prevent creating a new Klant
                afspraak.Klant = null; // This prevents EF from creating a new Klant based on the nested object
            }
            else if (afspraak.Klant != null)
            {
                // No KlantId provided but Klant object is present, create a new Klant
                _context.Klanten.Add(afspraak.Klant);
                await _context.SaveChangesAsync();
                afspraak.KlantId = afspraak.Klant.Id; // Link the newly created Klant to the Afspraak
            }
            // Continue with the assumption that either a KlantId was provided or a new Klant was created
            _context.Afspraken.Add(afspraak);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAfspraak), new { id = afspraak.Id }, afspraak);
        }

        // DELETE: api/Afspraaks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAfspraak(int id)
        {
            var afspraak = await _context.Afspraken.FindAsync(id);
            if (afspraak == null)
            {
                return NotFound();
            }

            _context.Afspraken.Remove(afspraak);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AfspraakExists(int id)
        {
            return _context.Afspraken.Any(e => e.Id == id);
        }
    }
}
