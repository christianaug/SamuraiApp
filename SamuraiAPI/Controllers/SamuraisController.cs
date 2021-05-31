using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamuraisController : ControllerBase
    {
        private readonly SamuraiContext _context;

        //every time an instance of this controller is used, the ConfigurationServicesMethod will setup the context for you
        public SamuraisController(SamuraiContext context)
        {
            _context = context;
        }

        // GET: api/Samurais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Samurai>>> GetSamurais()
        {
            return await _context.Samurais.ToListAsync();
        }

        // GET: api/Samurais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Samurai>> GetSamurai(int id)
        {
            var samurai = await _context.Samurais.FindAsync(id);

            if (samurai == null)
            {
                return NotFound();
            }

            return samurai;
        }

        // PUT: api/Samurais/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSamurai(int id, Samurai samurai)
        {
            if (id != samurai.Id)
            {
                return BadRequest();
            }

            _context.Entry(samurai).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SamuraiExists(id))
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

        // POST: api/Samurais
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Samurai>> PostSamurai(Samurai samurai)
        {
            _context.Samurais.Add(samurai);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSamurai", new { id = samurai.Id }, samurai);
        }

        // DELETE: api/Samurais/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Samurai>> DeleteSamurai(int id)
        {
            var samurai = await _context.Samurais.FindAsync(id);
            if (samurai == null)
            {
                return NotFound();
            }

            _context.Samurais.Remove(samurai);
            await _context.SaveChangesAsync();

            return samurai;
        }

        private bool SamuraiExists(int id)
        {
            return _context.Samurais.Any(e => e.Id == id);
        }

        //TEST METHOD CALLS ##################################################

        [HttpGet("test")]
        public ActionResult TestCode()
		{
            GetSamuraisCreatedPastWeek();
            return Ok();
		}

        private void AddNewSamuraiWithSecretIdentity()
        {
            var samurai = new Samurai { Name = "Juniper" };
            samurai.SecretIdentity = new SecretIdentity { RealName = "Julie" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private void AddSecretIdentityUsingSamurai()
		{
            var identity = new SecretIdentity { SamuraiId = 1, };
            _context.Add(identity);
            _context.SaveChanges();
		}

        private void AddSecretIdentityToExistingSamurai()
		{
            var samurai = _context.Samurais.Include(s => s.SecretIdentity).FirstOrDefault(s => s.Id == 37);
			Console.WriteLine(samurai.Name);
            samurai.SecretIdentity = new SecretIdentity { RealName = "John Shmeat" };
            _context.SaveChanges();
		}

        private void CreateNewSamurai()
		{
            var samurai = new Samurai { Name = "Ronin" };
            _context.Samurais.Add(samurai);
            var timestamp = DateTime.Now;
            _context.Entry(samurai).Property("Created").CurrentValue = timestamp;
            _context.Entry(samurai).Property("LastModified").CurrentValue = timestamp;
            _context.SaveChanges();
        }

        private void GetSamuraisCreatedPastWeek()
		{
            var oneWeekAgo = DateTime.Now.AddDays(-7);
            var newSamurais = _context.Samurais.Where(s => EF.Property<DateTime>(s, "Created") >= oneWeekAgo).Select(s => new { s.Id, s.Name, Created=EF.Property<DateTime>(s, "Created") }).ToList();

			foreach (var s in newSamurais)
			{
                Console.WriteLine($"{s.Id} - {s.Name} - {s.Created}");
			};
		}

    }
}
