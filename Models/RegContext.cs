using Microsoft.EntityFrameworkCore;

namespace LoginReg.Models
{
    public class RegContext : DbContext
    {
        public RegContext(DbContextOptions options) : base(options) {}
         //property will be a DbSet - a collection type from the Entity Framework library 
        //that you will provide your Model class in angle brackets
        //Your DBSet will refer to all data in your corresponding table as objects of the Model type you provide.
        public DbSet<User> Users {get; set;}
        // protected override void OnModelCreating(ModelBuilder builder)
        // {
        //     builder.Entity<User>()
        //         .HasIndex(u => u.Email)
        //         .IsUnique();
        // }


    }
}