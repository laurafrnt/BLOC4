using API.Model;
using Bogus;

public class DatabaseSeeder
{
    private readonly AppDbContext _context;

    public DatabaseSeeder(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        // Si la table Employees est vide, on génère des données factices
        if (!_context.Employees.Any())
        {
            var employeeFaker = new Faker<Employee>("fr") // Locale française
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(e => e.Email, f => f.Internet.Email())
                .RuleFor(e => e.Birthday, f => f.Date.Past(30, DateTime.Now.AddYears(-20)));

            var fakeEmployees = employeeFaker.Generate(50); // Génère 50 employés factices

            _context.Employees.AddRange(fakeEmployees);
            _context.SaveChanges();
        }
    }
}
