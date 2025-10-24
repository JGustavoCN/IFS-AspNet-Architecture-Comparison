using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IfsAcademicSystem.Mvc.Models
{
    // removido por causa do uso de herança TPH [Table("Estudantes")]
    public class Student : Person
    {
        // ID, LastName, FirstMidName e FullName são herdados de Person

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data da Matrícula")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Matrículas")]
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}