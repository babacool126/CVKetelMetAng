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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAfspraken()
        {
            var afspraken = await _context.Afspraken
                .Include(a => a.Klant) // Make sure to include the Klant entity
                .Select(a => new
                {
                    AfspraakId = a.Id,
                    Soort = a.Soort,
                    DatumTijd = a.DatumTijd,
                    KlantId = a.KlantId,
                    KlantNaam = a.Klant.Naam,
                    KlantEmail = a.Klant.Email,
                    KlantTelefoonnummer = a.Klant.Telefoonnummer
                })
                .ToListAsync();

            return Ok(afspraken);
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

        [HttpPost]
        public async Task<ActionResult<Afspraak>> PostAfspraak([FromBody] Afspraak afspraak)
        {
            // Check if Klant already exists
            var existingKlant = await _context.Klanten.FirstOrDefaultAsync(k => k.Email == afspraak.Klant.Email);
            if (existingKlant == null)
            {
                // Create new Klant if not exists
                Klant newKlant = new Klant
                {
                    Naam = afspraak.Klant.Naam,
                    Email = afspraak.Klant.Email,
                    Telefoonnummer = afspraak.Klant.Telefoonnummer
                };
                _context.Klanten.Add(newKlant);
                await _context.SaveChangesAsync();
                afspraak.KlantId = newKlant.Id; // Set KlantId for the new appointment
            }
            else
            {
                afspraak.KlantId = existingKlant.Id; // Use existing KlantId
            }

            // Clear Klant to avoid EF trying to create a new one due to navigation property
            afspraak.Klant = null;

            // Proceed to add the new Afspraak
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
