using CoursesWebsite.Data;
using CoursesWebsite.Models;

namespace CoursesWebsite.Areas.InstructorsArea.Data
{
    public interface IVideoService: IServiceData<Video>
    {
        public ICollection<Course> GetCourses();
    }
}
