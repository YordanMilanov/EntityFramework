using EntityRelations.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityRelations
{
    internal class ApplicationContext : DbContext
    {


     public DbSet<Student> Students { get; set; }
     public DbSet<Address> Adresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
            optionsBuilder.UseSqlServer("Server=DESKTOP-324OS5E;Database=EntityFrameworkRelations;Trusted_Connection=True;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>() //the student
                .HasOne(X => X.Address) //has one address
                .WithOne(x => x.Student) // and the address has only one student, not collection of students
                .HasForeignKey<Address>(x => x.StudentId); //the address entity has foreign key StudentId
        }
    }

 
}
