using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IfsAcademicSystem.RazorPages.Models
{
    [Table("Cursos")]
    [Index(nameof(Title), IsUnique = true)] // Garante que não haverá dois cursos com o mesmo título
    public class Course
    {
        // A aplicação fornecerá a chave primária, não o banco de dados.
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Código do Curso")]
        public int CourseID { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "O Título deve ter entre 3 e 50 caracteres.")]
        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Range(0, 5, ErrorMessage = "O valor dos créditos deve estar entre 0 e 5.")]
        [Required(ErrorMessage = "O campo Créditos é obrigatório.")]
        [Display(Name = "Créditos")]
        public int Credits { get; set; }

        // Chave estrangeira para a entidade Department
        public int DepartmentID { get; set; }

        // Propriedades de navegação
        public Department Department { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Instructor> Instructors { get; set; }
    }
}
