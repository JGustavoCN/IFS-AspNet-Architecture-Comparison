using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IfsAcademicSystem.Mvc.Models
{
    // removido por causa do uso de herança TPH [Table("Instrutores")]
    public class Instructor : Person
    {
        // ID, LastName, FirstMidName e FullName são herdados de Person

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de Contratação")]
        public DateTime HireDate { get; set; }
        [Display(Name = "Cursos")]
        public ICollection<Course> Courses { get; set; }
        [Display(Name = "Escritorio")]
        public OfficeAssignment OfficeAssignment { get; set; }
    }
}