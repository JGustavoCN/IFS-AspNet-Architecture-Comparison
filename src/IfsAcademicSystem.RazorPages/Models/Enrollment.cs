using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IfsAcademicSystem.RazorPages.Models
{
    public enum Grade { A, B, C, D, F }

    [Table("Matriculas")]
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        // Chaves Estrangeiras
        public int CourseID { get; set; }
        public int StudentID { get; set; }

        [Display(Name = "Nota")]
        [DisplayFormat(NullDisplayText = "Sem nota")]
        public Grade? Grade { get; set; }

        // Propriedades de Navegação com [ForeignKey] para clareza
        [ForeignKey("CourseID")] // 2. Liga explicitamente a propriedade de navegação à sua chave
        public Course Course { get; set; }

        [ForeignKey("StudentID")] // 2. Liga explicitamente a propriedade de navegação à sua chave
        public Student Student { get; set; }
    }
}
