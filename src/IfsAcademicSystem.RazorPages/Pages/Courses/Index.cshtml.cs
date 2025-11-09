using IfsAcademicSystem.Data.Data;
using IfsAcademicSystem.Data.Models;
using IfsAcademicSystem.Data.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IfsAcademicSystem.RazorPages.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly SchoolContext _context;

        public IndexModel(SchoolContext context)
        {
            _context = context;
        }

        public IList<CourseViewModel> CoursesVM { get; set; }

        public async Task OnGetAsync()
        {
            CoursesVM = await _context.Courses
                .Select(p => new CourseViewModel
                {
                    CourseID = p.CourseID,
                    Title = p.Title,
                    Credits = p.Credits,
                    DepartmentName = p.Department.Name // Projeta apenas o nome do departamento
                }).ToListAsync();
        }
    }
}
