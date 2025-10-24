using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IfsAcademicSystem.RazorPages.Pages.Courses
{
    public class UpdateCourseCreditsModel : PageModel
    {

        private readonly IfsAcademicSystem.RazorPages.Data.SchoolContext _context;

        public UpdateCourseCreditsModel(IfsAcademicSystem.RazorPages.Data.SchoolContext context)
        {
            _context = context;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? multiplier) 
        {
            
            if (multiplier != null)
            {
                ViewData["RowsAffected"] =
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE Cursos SET Credits = Credits * {0}",
                        parameters: multiplier);
            }
            return Page();
            
        }
    }
}
