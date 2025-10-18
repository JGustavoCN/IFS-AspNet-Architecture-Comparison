using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IfsAcademicSystem.RazorPages.Models
{
    [Table("Cursos")]
    [Index(nameof(Title), IsUnique = true)] // 1. Garante que não haverá dois cursos com o mesmo título
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Código do Curso")]
        public int CourseID { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O Título deve ter entre 3 e 50 caracteres.")]
        [Column(TypeName = "varchar(50)")] // 3. Otimiza o tipo de dado no banco
        public string Title { get; set; }

        [Display(Name = "Créditos")]
        [Range(0, 5, ErrorMessage = "O valor dos créditos deve estar entre 0 e 5.")]
        public int Credits { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
