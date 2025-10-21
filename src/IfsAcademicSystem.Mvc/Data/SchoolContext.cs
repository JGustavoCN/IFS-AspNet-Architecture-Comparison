using IfsAcademicSystem.Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace IfsAcademicSystem.Mvc.Data
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeia as entidades para tabelas com nomes em português.
            modelBuilder.Entity<Course>().ToTable("Cursos");
            modelBuilder.Entity<Enrollment>().ToTable("Matriculas");
            modelBuilder.Entity<Student>().ToTable("Estudantes");
            modelBuilder.Entity<Department>().ToTable("Departamentos");
            modelBuilder.Entity<Instructor>().ToTable("Instrutores");
            modelBuilder.Entity<OfficeAssignment>().ToTable("Escritorios");

            // Configura a relação muitos-para-muitos entre Course e Instructor.
            // O EF Core criará uma tabela de junção chamada "CourseInstructor".
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors)
                .WithMany(i => i.Courses);

            modelBuilder.Entity<Department>()
                .HasMany(d => d.Courses)
                .WithOne(c => c.Department)
                .OnDelete(DeleteBehavior.Restrict); // Impede a exclusão de um departamento se ele tiver cursos.
        }
    }
}