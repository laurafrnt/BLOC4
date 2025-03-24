namespace API.Model
{
    public class EmployeeSite
    {
        public int EmployeeId { get; set; }
        public int SiteId {  get; set; }
        public Employee Employee { get; set; }
        public Site Site { get; set; }
    }
}
