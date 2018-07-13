using Microsoft.EntityFrameworkCore;

namespace contact_app.Models
{
    public class ContactAppContext : DbContext
    {
          public ContactAppContext(DbContextOptions<ContactAppContext> options)
            : base(options)
        {
        }

        
         public DbSet<Person> Person { get; set; }
          public DbSet<Customer> Customer { get; set; }
           public DbSet<Supplier> Supplier { get; set; }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .ToTable("People");
        modelBuilder.Entity<Person>()
        .HasKey(p => p.Id);

        modelBuilder.Entity<Person>()
        .HasOne(s => s.Customer)
        .WithOne(ad => ad.Person);

        modelBuilder.Entity<Person>()
        .HasOne(s => s.Supplier)
        .WithOne(ad => ad.Person);

        modelBuilder.Entity<Customer>()
        .ToTable("Customer");

        modelBuilder.Entity<Customer>().HasKey(p => p.Id);

        modelBuilder.Entity<Supplier>()
        .ToTable("Supplier");

        modelBuilder.Entity<Supplier>().HasKey(p => p.Id);
    }

    }
}