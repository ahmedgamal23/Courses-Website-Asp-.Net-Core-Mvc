using CoursesWebsite.Models;

namespace CoursesWebsite.ViewModels
{
    public class CourseEnrollmentViewModel
    {
        public Course? Course { get; set; }
        public bool isEnrolled { get; set; }
    }
}
