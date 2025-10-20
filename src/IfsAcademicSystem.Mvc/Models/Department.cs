using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IfsAcademicSystem.RazorPages.Models
{
    [Table("Departamentos")]
    public class Department
    {
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "O Nome deve ter entre 3 e 50 caracteres.")]
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "O campo Orçamento é obrigatório.")]
        [Display(Name = "Orçamento")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "O campo Data de Início é obrigatório.")]
        [Display(Name = "Data de Início")]
        public DateTime StartDate { get; set; }

        public int? InstructorID { get; set; }

        [Timestamp]
        //[Display(Name = "Token")]
        public byte[] ConcurrencyToken { get; set; }

        [Display(Name = "Administrador")]
        public Instructor Administrator { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}