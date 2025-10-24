using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; // Necessário para EF.Property
using IfsAcademicSystem.RazorPages.Data;
using IfsAcademicSystem.RazorPages.Models;

namespace IfsAcademicSystem.RazorPages.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly SchoolContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(SchoolContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Student> Students { get; set; }

        // O método OnGetAsync foi atualizado
        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;

            // 1. ATUALIZADO: Usando nomes de propriedades reais, igual ao MVC
            NameSort = String.IsNullOrEmpty(sortOrder) ? "LastName_desc" : "";
            DateSort = sortOrder == "EnrollmentDate" ? "EnrollmentDate_desc" : "EnrollmentDate";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Student> studentsIQ = from s in _context.Students
                                             select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                studentsIQ = studentsIQ.Where(s => s.LastName.Contains(searchString)
                                                || s.FirstMidName.Contains(searchString));
            }

            // 2. ATUALIZADO: O switch foi substituído pela lógica dinâmica do MVC

            // Define a ordenação padrão se nenhuma for passada
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "LastName";
            }

            bool descending = false;
            if (sortOrder.EndsWith("_desc"))
            {
                sortOrder = sortOrder.Substring(0, sortOrder.Length - 5); // Remove o "_desc"
                descending = true;
            }

            if (descending)
            {
                studentsIQ = studentsIQ.OrderByDescending(e => EF.Property<object>(e, sortOrder));
            }
            else
            {
                studentsIQ = studentsIQ.OrderBy(e => EF.Property<object>(e, sortOrder));
            }

            // 3. O resto do código (paginação) permanece o mesmo
            var pageSize = Configuration.GetValue("PageSize", 4);
            Students = await PaginatedList<Student>.CreateAsync(
                studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}