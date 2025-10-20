using System.ComponentModel.DataAnnotations;

namespace IfsAcademicSystem.RazorPages.Models.SchoolViewModels
{
    public class InstructorIndexData
    {
        [Display(Name = "Instrutores")]
        public IEnumerable<Instructor> Instructors { get; set; }

        [Display(Name = "Cursos")]
        public IEnumerable<Course> Courses { get; set; }

        [Display(Name = "Matrículas")]
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
