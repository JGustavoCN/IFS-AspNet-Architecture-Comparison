using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IfsAcademicSystem.RazorPages.Data;
using IfsAcademicSystem.RazorPages.Models;

namespace IfsAcademicSystem.RazorPages.Pages.Instructors
{
    public class DetailsModel : PageModel
    {
        private readonly IfsAcademicSystem.RazorPages.Data.SchoolContext _context;

        public DetailsModel(IfsAcademicSystem.RazorPages.Data.SchoolContext context)
        {
            _context = context;
        }

        public Instructor Instructor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            else
            {
                Instructor = instructor;
            }
            return Page();
        }
    }
}
