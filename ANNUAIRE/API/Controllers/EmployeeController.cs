using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Injectez AppDbContext via le constructeur
        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // GET all employees
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> Get()
        {
            var employees = _context.Employees.ToList();
            return Ok(employees);
        }

        // GET an employee by id
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();  // Si l'employé n'est pas trouvé, retourner une réponse 404
            }
            return Ok(employee);
        }

        // PUT (update) an employee
        [HttpPut]
        public ActionResult<Employee> PutEmployee(Employee employee)
        {
            var existingEmployee = _context.Employees.Find(employee.IdEmployee);
            if (existingEmployee == null)
            {
                return NotFound();  // Si l'employé n'est pas trouvé, retourner une réponse 404
            }

            _context.Entry(existingEmployee).CurrentValues.SetValues(employee);
            _context.SaveChanges();
            return Ok(employee);
        }

        // DELETE an employee
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

        // POST (add) a new employee
        [HttpPost]
        public ActionResult<Employee> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.IdEmployee }, employee);
        }
    }
}
