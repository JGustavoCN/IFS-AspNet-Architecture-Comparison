using IfsAcademicSystem.RazorPages.Data;
using IfsAcademicSystem.RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IfsAcademicSystem.RazorPages.Pages.Departments
{
    public class DeleteModel : PageModel
    {
        private readonly IfsAcademicSystem.RazorPages.Data.SchoolContext _context;

        public DeleteModel(IfsAcademicSystem.RazorPages.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; }
        public string ConcurrencyErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, bool? concurrencyError)
        {
            Department = await _context.Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (Department == null)
            {
                return NotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ConcurrencyErrorMessage = "O registro que você tentou excluir foi modificado por outro usuário "
                  + "após você selecionar 'Excluir'. A operação de exclusão foi cancelada e os valores atuais "
                  + "do banco de dados foram exibidos. Se você ainda deseja excluir este registro, "
                  + "clique no botão 'Excluir' novamente.";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                // Busca o departamento a ser excluído pelo ID.
                var departmentToDelete = await _context.Departments.FindAsync(id);

                if (departmentToDelete == null)
                {
                    // Se o departamento já foi excluído, retorna para a lista.
                    return RedirectToPage("./Index");
                }

                // Anexa a entidade com o token de concorrência vindo do formulário.
                _context.Attach(Department).State = EntityState.Deleted;
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                // Se ocorrer um erro de concorrência, recarrega a página de exclusão
                // com uma mensagem de erro.
                return RedirectToPage("./Delete",
                    new { concurrencyError = true, id = id });
            }
        }
    }
}