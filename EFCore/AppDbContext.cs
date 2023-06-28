using Microsoft.EntityFrameworkCore;


namespace EFCore
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=EFCore;Integrated Security=True;");

        public DbSet<Employee> Employees { get; set;}
    }
}
