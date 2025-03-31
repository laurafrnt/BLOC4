using API;  // Assurez-vous que ce namespace est correct
using ClassLibrary;  // Si DesignTimeDbContextFactory et SeederDbFaker sont dans ce namespace

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Faker Databases");
        DesignTimeDbContextFactory designTimeDbContextFactory = new DesignTimeDbContextFactory();
        SeederDbFaker seederDatabaseFaker = new SeederDbFaker(designTimeDbContextFactory.CreateDbContext(args));
        Task task = seederDatabaseFaker.SeedDatabaseAsync();
    }
}
