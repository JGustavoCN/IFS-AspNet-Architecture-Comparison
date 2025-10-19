using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IfsAcademicSystem.RazorPages.Models
{
    [Table("Escritorios")]
    public class OfficeAssignment
    {
        // Chave Primária e Estrangeira (Relação 1-para-1 com Instructor)
        [Key]
        public int InstructorID { get; set; }

        [RegularExpression(@"^Bloco [A-Z] - Sala \d{3}$", ErrorMessage = "O formato deve ser 'Bloco X - Sala YYY'.")]
        [StringLength(50, ErrorMessage = "A localização não pode exceder 50 caracteres.")]
        [Required(ErrorMessage = "O campo Localização do Escritório é obrigatório.")]
        [Display(Name = "Localização do Escritório")]
        public string Location { get; set; }

        // Propriedade de navegação para o Instrutor
        public Instructor Instructor { get; set; }
    }
}
