using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using IfsAcademicSystem.RazorPages.Data;
using IfsAcademicSystem.RazorPages.Models;

namespace IfsAcademicSystem.RazorPages.Pages.Instructors
{
    public class CreateModel : PageModel
    {
        private readonly IfsAcademicSystem.RazorPages.Data.SchoolContext _context;

        public CreateModel(IfsAcademicSystem.RazorPages.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Instructor Instructor { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Instructors.Add(Instructor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
