namespace API.Model
{
    public class EmployeeService
    {
        public int EmployeeId { get; set; }
        public int ServiceId { get; set; }
        public Employee Employee { get; set; } // Only one relation with an employee
        public Service Service { get; set; } // Only one relation with a service 
    }
}

