using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Injectez AppDbContext via le constructeur
        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        // GET services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            var services = await _context.Services.ToListAsync();
            return Ok(services);
        }

        // GET service
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        // PUT (update) service
        [HttpPut("{id}")]
        public async Task<ActionResult<Service>> PutService(int id, Service service)
        {
            if (id != service.IdService)
            {
                return BadRequest("ID mismatch");
            }

            var existingService = await _context.Services.FindAsync(id);
            if (existingService == null)
            {
                return NotFound();
            }

            // Mettez à jour les propriétés du service
            existingService.Name = service.Name;

            await _context.SaveChangesAsync();
            return Ok(existingService);
        }

        // DELETE service
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            // Vérifiez si des employés sont associés au service
            bool hasEmployees = await _context.Employees.AnyAsync(e => e.IdService == id);
            if (hasEmployees)
            {
                return BadRequest("Suppression du Service impossible, le Service est rataché à au moins un employé.");
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST (add) service
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetService), new { id = service.IdService }, service);
        }
    }
}
