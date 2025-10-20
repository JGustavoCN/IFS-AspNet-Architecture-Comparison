using System.ComponentModel.DataAnnotations;

namespace IfsAcademicSystem.RazorPages.Models.SchoolViewModels
{
    public class CourseViewModel
    {
        [Display(Name = "Código")]
        public int CourseID { get; set; }

        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Créditos")]
        public int Credits { get; set; }

        [Display(Name = "Departamento")]
        public string DepartmentName { get; set; }
    }
}