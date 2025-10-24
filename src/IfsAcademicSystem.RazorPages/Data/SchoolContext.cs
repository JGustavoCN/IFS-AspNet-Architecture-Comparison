using IfsAcademicSystem.RazorPages.Models;
using Microsoft.EntityFrameworkCore;

namespace IfsAcademicSystem.RazorPages.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Person> People { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeia as entidades para tabelas com nomes em português.
            modelBuilder.Entity<Course>().ToTable("Cursos");
            modelBuilder.Entity<Enrollment>().ToTable("Matriculas");
            // removido por causa do uso de herança TPH modelBuilder.Entity<Student>().ToTable("Estudantes");
            modelBuilder.Entity<Department>().ToTable("Departamentos");
            // removido por causa do uso de herança TPH  modelBuilder.Entity<Instructor>().ToTable("Instrutores");
            modelBuilder.Entity<OfficeAssignment>().ToTable("Escritorios");
            modelBuilder.Entity<Person>().ToTable("Pessoas");

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors)
                .WithMany(i => i.Courses);

            modelBuilder.Entity<Department>()
                .HasMany(d => d.Courses)
                .WithOne(c => c.Department)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}