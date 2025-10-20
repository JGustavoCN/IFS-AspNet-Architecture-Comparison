using System.ComponentModel.DataAnnotations;

namespace IfsAcademicSystem.RazorPages.Models.SchoolViewModels
{
    public class AssignedCourseData
    {
        public int CourseID { get; set; }

        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Atribuído")]
        public bool Assigned { get; set; }
    }
}
