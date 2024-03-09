using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CVKetelMetAng.Data;
using CVKetelMetAng.Models;

namespace CVKetelMetAng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KlantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Klants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Klant>>> GetKlanten()
        {
            return await _context.Klanten.ToListAsync();
        }

        // GET: api/Klants/EmailExists?email=value
        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            return await _context.Klanten.AnyAsync(klant => klant.Email == email);
        }

        // GET: api/Klants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Klant>> GetKlant(int id)
        {
            var klant = await _context.Klanten.FindAsync(id);

            if (klant == null)
            {
                return NotFound();
            }

            return klant;
        }

        // PUT: api/Klants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKlant(int id, Klant klant)
        {
            if (id != klant.Id)
            {
                return BadRequest();
            }

            _context.Entry(klant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KlantExists(id))
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

        // POST: api/Klants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Klant>> PostKlant(Klant klant)
        {
            _context.Klanten.Add(klant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKlant", new { id = klant.Id }, klant);
        }

        // DELETE: api/Klants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKlant(int id)
        {
            var klant = await _context.Klanten.FindAsync(id);
            if (klant == null)
            {
                return NotFound();
            }

            _context.Klanten.Remove(klant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KlantExists(int id)
        {
            return _context.Klanten.Any(e => e.Id == id);
        }
    }
}
