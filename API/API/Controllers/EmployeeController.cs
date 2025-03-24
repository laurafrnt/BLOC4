using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.Eventing.Reader;

namespace API.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class EmployeeController : ControllerBase
    {

        // SHOW all employees
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> Get()
        {
            using (AppDbContext appContext = new AppDbContext())
            {
                var employees = appContext.Employees.ToList();
                return Ok(employees);
            }
        }

        // SHOW an employee with paramater id
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployee(int id)
        {
            using (AppDbContext appContext = new AppDbContext())
            {
                var employee = appContext.Employees.Find(id);
                return Ok(employee);
            }
        }

        // UPDATE an employee
        [HttpPut]
        public ActionResult<Employee> PutEmployee(Employee employee)
        {
            using (AppDbContext appContext = new AppDbContext())
            {
                appContext.Employees.Update(employee);
                appContext.SaveChanges();
                return Ok(employee);
            }
        }

        // DELETE an employee
        [HttpDelete]
        public ActionResult<Employee> DeleteEmployee(Employee employee)
        {
            using (AppDbContext appContext = new AppDbContext())
            {
                appContext.Employees.Remove(employee);
                appContext.SaveChanges();
                return Ok();
            }
        }

        // ADD an employee
        [HttpPost]
        public ActionResult<Employee> PostEmployee(Employee employee)
        {
            using (AppDbContext appContext = new AppDbContext())
            {
                appContext.Employees.Add(employee);
                appContext.SaveChanges();
                return Ok(employee);
            }

        }

    }
}
