using System.Data.Entity;

namespace PersonCSV.Models
{
    public class PersonDbContext : DbContext
    {
        public PersonDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}