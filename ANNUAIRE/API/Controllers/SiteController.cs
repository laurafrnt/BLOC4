using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SiteController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Injectez AppDbContext via le constructeur
        public SiteController(AppDbContext context)
        {
            _context = context;
        }

        // GET sites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Site>>> GetSites()
        {
            var sites = await _context.Sites.ToListAsync();
            return Ok(sites);
        }

        // GET site
        [HttpGet("{id}")]
        public async Task<ActionResult<Site>> GetSite(int id)
        {
            var site = await _context.Sites.FindAsync(id);

            if (site == null)
            {
                return NotFound();
            }

            return Ok(site);
        }

        // PUT (update) site
        [HttpPut("{id}")]
        public async Task<ActionResult<Site>> PutSite(int id, Site site)
        {
            if (id != site.IdSite)
            {
                return BadRequest("ID mismatch");
            }

            var existingSite = await _context.Sites.FindAsync(id);
            if (existingSite == null)
            {
                return NotFound();
            }

            // Mettez à jour les propriétés du site
            existingSite.City = site.City;

            await _context.SaveChangesAsync();
            return Ok(existingSite);
        }

        // DELETE site
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSite(int id)
        {
            var site = await _context.Sites.FindAsync(id);
            if (site == null)
            {
                return NotFound();
            }

            // Vérifiez si des employés sont associés au site
            bool hasEmployees = await _context.Employees.AnyAsync(e => e.IdSite == id);
            if (hasEmployees)
            {
                return BadRequest("Suppression du Site impossible, le site est rataché à au moins un employé.");
            }

            _context.Sites.Remove(site);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST (add) site
        [HttpPost]
        public async Task<ActionResult<Site>> PostSite(Site site)
        {
            _context.Sites.Add(site);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSite), new { id = site.IdSite }, site);
        }
    }
}
