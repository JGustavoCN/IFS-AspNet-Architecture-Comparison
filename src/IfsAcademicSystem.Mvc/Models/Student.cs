using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IfsAcademicSystem.Mvc.Models
{
    [Table("Estudantes")]
    public class Student
    {
        public int ID { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "O nome deve conter apenas letras e começar com maiúscula.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O Sobrenome deve ter entre 2 e 50 caracteres.")]
        [Required(ErrorMessage = "O campo Sobrenome é obrigatório.")]
        [Display(Name = "Sobrenome")]
        public string LastName { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "O nome deve conter apenas letras  e começar com maiúscula.")]
        [StringLength(50, ErrorMessage = "O Nome não pode ter mais de 50 caracteres.")]
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [Column("FirstName")] 
        [Display(Name = "Nome")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data da Matrícula")]
        public DateTime EnrollmentDate { get; set; }

        [NotMapped]
        [Display(Name = "Nome Completo")]
        public string FullName
        {
            get { return $"{FirstMidName} {LastName}"; }
        }
        [Display(Name = "Matrículas")]
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}