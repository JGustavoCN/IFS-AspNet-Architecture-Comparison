using System.ComponentModel.DataAnnotations;

namespace IfsAcademicSystem.Mvc.Models.SchoolViewModels
{
    public class EnrollmentDateGroup
    {
        // Define o formato de exibição e o texto do rótulo para a data.
        [Display(Name = "Data da Matrícula")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EnrollmentDate { get; set; }

        // Define o texto do rótulo para a contagem de alunos.
        [Display(Name = "Número de Alunos")]
        public int StudentCount { get; set; }
    }
}
