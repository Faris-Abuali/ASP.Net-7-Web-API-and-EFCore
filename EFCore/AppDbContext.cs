using Microsoft.EntityFrameworkCore;
using EFCore.Models;
using EFCore.Configurations;

namespace EFCore
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=EFCore;Integrated Security=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // FluentAPI configuration for Blog domain model class - in separate class.
            new BlogEntityTypeConfiguration().Configure(modelBuilder.Entity<Blog>());

            // OR you can say:
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogEntityTypeConfiguration).Assembly);

            // -- Parts 9 & 10
            modelBuilder.Entity<AuditEntry>(); // will recognize the `AuditEntry` class as domain model class
                                               //modelBuilder.Ignore<Post>(); // will exclude the `Post` class from domain model classes

            // Blogs table will still in the DB but will be excluded from all migrations - Entity Framework won't listen to it.
            //modelBuilder.Entity<Blog>()
            //    .ToTable("Blogs", b => b.ExcludeFromMigrations());

            // Change the table name to `Posts`
            //modelBuilder.Entity<Post>()
            //    .ToTable("Posts");

            modelBuilder.Entity<Post>()
                .ToTable("Posts", schema: "blogging");

            //modelBuilder.Entity<Post>()
            //    .ToView("SelectPosts", schema: "blogging");

            //modelBuilder.HasDefaultSchema("blogging"); // Will make the default schema for any new table as "blogging.TableName"


            // Ignore `AddedOn` Property
            modelBuilder.Entity<Blog>()
                .Ignore(b => b.AddedOn);

            //// Rename column
            //modelBuilder.Entity<Blog>()
            //    .Property(m => m.Url)
            //    .HasColumnName("BlogUrl");



            //modelBuilder.Entity<Blog>(e =>
            //{
            //    e.Property(m => m.Url).HasColumnType("varchar(200)");
            //    e.Property(m => m.Rating).HasColumnType("decimal(5,2)");
            //});


            // Add primary key to an Entity
            modelBuilder.Entity<Book>()
                .HasKey(b => b.key)
                .HasName("PK_BookKey");

            // Composite key (Name & Author)
            modelBuilder.Entity<Book>()
            .HasKey(b => new { b.Name, b.Author });
        }

        //public DbSet<Employee> Employees { get; set;}
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
