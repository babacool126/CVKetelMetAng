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
                    KlantTelefoonnummer = a.Klant.Telefoonnummer,
                    KlantAdres = a.Klant.Adres
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
        public async Task<IActionResult> PutAfspraak(int id, [FromBody] Afspraak afspraak)
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
        public async Task<IActionResult> PostAfspraak([FromBody] AppointmentCreationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Ensure the appointment date and time are in the future
            if (model.AppointmentDateTime <= DateTime.Now)
            {
                return BadRequest("The appointment must be scheduled for a future date and time.");
            }

            // Weekday check
            if (model.AppointmentDateTime.DayOfWeek == DayOfWeek.Saturday ||
                model.AppointmentDateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                return BadRequest("Appointments can only be scheduled from Monday to Friday.");
            }

            // Time slot check (assuming model.AppointmentDateTime is in local time)
            TimeSpan appointmentTime = model.AppointmentDateTime.TimeOfDay;
            TimeSpan startTime = new TimeSpan(8, 0, 0); // 8 AM
            TimeSpan endTime = new TimeSpan(18, 0, 0); // 6 PM
            if (appointmentTime < startTime || appointmentTime > endTime)
            {
                return BadRequest("Appointments can only be scheduled between 8 AM and 6 PM.");
            }

            // Appointment count check
            int existingAppointmentsCount = await _context.Afspraken.CountAsync(
                a => a.DatumTijd.Date == model.AppointmentDateTime.Date);
            if (existingAppointmentsCount >= 4) // Assuming a total of 4 appointments per day as an example
            {
                return BadRequest("The maximum number of appointments for this day has been reached.");
            }

            var existingKlant = await _context.Klanten.FirstOrDefaultAsync(k => k.Email == model.CustomerEmail);

            // If Klant exists, proceed to create a new Afspraak for them
            if (existingKlant != null)
            {
                // Create a new Afspraak for the existing Klant
                var afspraak = new Afspraak
                {
                    KlantId = existingKlant.Id,
                    Soort = model.AppointmentType,
                    DatumTijd = model.AppointmentDateTime
                };

                _context.Afspraken.Add(afspraak);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAfspraak), new { id = afspraak.Id }, afspraak);
            }
            else
            {
                // If Klant does not exist, create a new Klant and Afspraak
                var newKlant = new Klant
                {
                    Naam = model.CustomerName,
                    Email = model.CustomerEmail,
                    Telefoonnummer = model.CustomerPhoneNumber,
                    Adres = model.CustomerAddress
                };

                _context.Klanten.Add(newKlant);
                await _context.SaveChangesAsync(); // Save to ensure newKlant gets an ID

                var newAfspraak = new Afspraak
                {
                    KlantId = newKlant.Id,
                    Soort = model.AppointmentType,
                    DatumTijd = model.AppointmentDateTime
                };

                _context.Afspraken.Add(newAfspraak);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAfspraak), new { id = newAfspraak.Id }, newAfspraak);
            }
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
