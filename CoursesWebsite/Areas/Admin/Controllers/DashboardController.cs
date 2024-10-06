using CoursesWebsite.Areas.User.Data;
using CoursesWebsite.Data;
using CoursesWebsite.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoursesWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CoursesWebsiteDbContext _coursesWebsiteDbContext;

        public DashboardController(UserManager<ApplicationUser> userManager, CoursesWebsiteDbContext coursesWebsiteDbContext)
        {
            _userManager = userManager;
            _coursesWebsiteDbContext = coursesWebsiteDbContext;
        }

        public async Task<IActionResult> Index()
        {
            int course = _coursesWebsiteDbContext.Courses.Count();
            int category = _coursesWebsiteDbContext.Categories.Count();
            var instructors = await _userManager.GetUsersInRoleAsync("Instructor");
            var users = await _userManager.GetUsersInRoleAsync("User");
            var admin = await _userManager.GetUsersInRoleAsync("Admin");

            DashboardCounterViewModel viewModel = new DashboardCounterViewModel
            {
                CategoryCount = category,
                CourseCount = course,
                InstructorCount = instructors.Count(),
                UserCount = users.Count(),
                AdminCount = admin.Count()
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ShowInstructors()
        {
            var users = _userManager.Users.ToList();
            var instructors = new List<InstructorInfoViewModel>();
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Instructor"))
                    instructors.Add(new InstructorInfoViewModel()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email!,
                        Role = "Instructor"
                    });
            }
            return View(instructors);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("User ID is required.");
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            if (ModelState.IsValid)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }



    }
}
