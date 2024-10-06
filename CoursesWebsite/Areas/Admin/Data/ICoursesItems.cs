using CoursesWebsite.Areas.User.Data;
using CoursesWebsite.Data;
using CoursesWebsite.Models;
using Microsoft.AspNetCore.Identity;

namespace CoursesWebsite.Areas.Admin.Data
{
    public interface ICoursesItems : IServiceData<Course>
    {
        public IEnumerable<Category> GetCategories();
        public IEnumerable<ApplicationUser> GetAllUsers();
        public IEnumerable<IdentityRole> GetAllRoles();
    }
}
