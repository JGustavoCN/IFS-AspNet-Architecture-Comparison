using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IfsAcademicSystem.RazorPages.Models
{
    [Table("Estudantes")]
    public class Student
    {
        public int ID { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "O campo Sobrenome é obrigatório.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O Sobrenome deve ter entre 2 e 50 caracteres.")]
        public string LastName { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(50, ErrorMessage = "O Nome não pode exceder 50 caracteres.")]
        [Column("FirstName")] // Mapeia para uma coluna chamada "FirstName" no banco
        public string FirstMidName { get; set; }

        [Display(Name = "Data da Matrícula")]
        [DataType(DataType.Date)] // Gera um input de data na UI
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

        [NotMapped] // Esta propriedade não vira uma coluna no banco de dados
        [Display(Name = "Nome Completo")]
        public string FullName
        {
            get { return $"{FirstMidName} {LastName}"; }
        }

        // Propriedade de Navegação para a relação com Matrículas
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
