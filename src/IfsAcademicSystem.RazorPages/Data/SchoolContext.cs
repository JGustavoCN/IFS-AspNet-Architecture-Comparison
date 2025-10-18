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

        public DbSet<IfsAcademicSystem.RazorPages.Models.Student> Student { get; set; } = default!;
    }
}
