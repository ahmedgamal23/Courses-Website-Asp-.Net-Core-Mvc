using CoursesWebsite.Areas.Identity.Data;
using CoursesWebsite.Areas.User.Data;
using CoursesWebsite.Data;
using CoursesWebsite.Models;
using CoursesWebsite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoursesWebsite.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class CourseController : Controller
    {
        private readonly IServiceData<Course> _courseService;
        private readonly IServiceData<Enrollment> _enrollService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(IServiceData<Course> courseService,
                                IServiceData<Enrollment> enrollService,
                                UserManager<ApplicationUser> userManager)
        {
            _courseService = courseService;
            _enrollService = enrollService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userID = _userManager.GetUserId(User); // Get the current logged-in user's ID
            var courses = _courseService.GetItems();

            var courseEnrollModel = courses.Select(course => new CourseEnrollmentViewModel
            {
                Course = course,
                isEnrolled = _enrollService.GetItems().Any(x => x.UserId == userID && x.CourseId == course.Id)
            }).ToList();

            return View(courseEnrollModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enrollment(string id)
        {
            var userID = _userManager.GetUserId(User); // Get the current logged-in user's ID
            if (ModelState.IsValid)
            {
                var course = _courseService.GetItem(id);

                if (course == null)
                    return RedirectToAction("Index");

                Enrollment enrollment = new Enrollment()
                {
                    Course = course,
                    CourseId = course.Id,
                    UserId = userID,
                    EnrollmentDate = DateTime.Now,
                    isEnrolled = true
                };
                bool result = await _enrollService.Create(enrollment);
                if (result)
                    return RedirectToAction("Index", "Course");
            }
            return View("Index");
        }

    }
}
