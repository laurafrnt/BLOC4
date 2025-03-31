using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Injectez AppDbContext via le constructeur
        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // GET employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees
                .Include(e => e.Site)   // Charge les informations du site
                .Include(e => e.Service) // Charge les informations du service
                .ToListAsync();
        }

        // GET employee
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Site)   // Charge les infos du site
                .Include(e => e.Service) // Charge les infos du service
                .FirstOrDefaultAsync(e => e.IdEmployee == id); 

            if (employee == null)
            {
                return NotFound();  // Retourne 404 si l'employé n'existe pas
            }

            return Ok(employee);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> PutEmployee(int id, Employee employee)
        {
            if (id != employee.IdEmployee)
            {
                return BadRequest("ID mismatch");
            }

            var existingEmployee = await _context.Employees
                .Include(e => e.Site)  // Inclure les relations pour assurer l'intégrité
                .Include(e => e.Service)
                .FirstOrDefaultAsync(e => e.IdEmployee == id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            // Mettez à jour les propriétés de l'employé
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.PhoneNumber = employee.PhoneNumber;
            existingEmployee.Email = employee.Email;
            existingEmployee.Birthday = employee.Birthday;

            // Mettez à jour les relations (les IDs seulement)
            if (employee.IdSite != 0)
            {
                existingEmployee.IdSite = employee.IdSite;
            }

            if (employee.IdService != 0)
            {
                existingEmployee.IdService = employee.IdService;
            }

            // Sauvegardez les changements
            await _context.SaveChangesAsync();

            return Ok(existingEmployee);
        }


        // DELETE employee
        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();  // Si l'employé n'est pas trouvé, retourner une réponse 404
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return Ok();
        }

        // POST (add) employee
        [HttpPost]
        public ActionResult<Employee> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.IdEmployee }, employee);
        }
    }
}
