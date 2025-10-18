using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IfsAcademicSystem.RazorPages.Models;

namespace IfsAcademicSystem.RazorPages.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext (DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Cursos");
            modelBuilder.Entity<Enrollment>().ToTable("Matriculas");
            modelBuilder.Entity<Student>().ToTable("Estudantes");
        }
    }
}
