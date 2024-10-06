using CoursesWebsite.Areas.Admin.Data;
using CoursesWebsite.Areas.Identity.Data;
using CoursesWebsite.Areas.User.Data;
using CoursesWebsite.Data;
using CoursesWebsite.Models;
using CoursesWebsite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CoursesWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CourseController : Controller
    {
        private readonly ICoursesItems _courseItem;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CourseController(ICoursesItems courseItem, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager, 
                                                        RoleManager<IdentityRole> roleManager)
        {
            _courseItem = courseItem;
            _environment = environment;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var courses = _courseItem.GetItems();
            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var users = await _userManager.GetUsersInRoleAsync("Instructor");

            var categories = _courseItem.GetCategories().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

            var instructors = users.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Email
            });

            ViewBag.Categories = new SelectList(categories, "Value", "Text");
            ViewBag.Instructors = new SelectList(instructors, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            bool result = await _courseItem.Create(course);
            if (result)
            {
                return RedirectToAction("Index", "Course");
            }

            var users = await _userManager.GetUsersInRoleAsync("Instructor");
            var instructors = users.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Email
            });
            // Repopulate CategoryList if validation fails
            var categories = _courseItem.GetCategories().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

            ViewBag.Categories = new SelectList(categories, "Value", "Text");
            ViewBag.Instructors = new SelectList(instructors, "Value", "Text");
            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var course = _courseItem.GetItem(id);
            if (course != null)
            {
                var users = await _userManager.GetUsersInRoleAsync("Instructor");
                var categories = _courseItem.GetCategories().Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });

                var instructors = users.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Email
                });

                ViewBag.Categories = new SelectList(categories, "Value", "Text", course.CategoryId);
                ViewBag.Instructors = new SelectList(instructors, "Value", "Text", course.InstructorId);
                return View(course);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                var result = await _courseItem.Edit(course);
                if (result)
                    return RedirectToAction("Index");
            }

            var users = await _userManager.GetUsersInRoleAsync("Instructor");
            var categories = _courseItem.GetCategories().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

            var instructors = users.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Email
            });
            ViewBag.Categories = new SelectList(categories, "Value", "Text", course.CategoryId);
            ViewBag.Instructors = new SelectList(instructors, "Value", "Text", course.InstructorId);

            return View(course);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            bool result = _courseItem.Delete(id);
            if(result)
                return RedirectToAction("Index");
            return View();
        }

        [HttpGet]
        public IActionResult AddNewInstructor()
        {
            var users = _courseItem.GetAllUsers().Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.Email
            });

            var roles = _courseItem.GetAllRoles().Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.Name
            });

            ViewBag.Emails = new SelectList(users, "Value", "Text");
            ViewBag.RolesName = new SelectList(roles, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewInstructor(InstructorViewModel instructorViewModel)
        {
            if (ModelState.IsValid)
            {
                // change role for user to selected role
                var user = await _userManager.FindByIdAsync(instructorViewModel.UserId);
                if(user != null)
                {
                    // Check if the user is in the "User" role
                    if (await _userManager.IsInRoleAsync(user, "User"))
                    {
                        // Remove the "User" role
                        await _userManager.RemoveFromRoleAsync(user, "User");
                    }

                    // Add the "Instructor" role
                    if (!await _userManager.IsInRoleAsync(user, "Instructor"))
                    {
                        await _userManager.AddToRoleAsync(user, "Instructor");
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"User not found {instructorViewModel.UserId}");
                    return View();
                }
            }

            var users = _courseItem.GetAllUsers().Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.Email
            });

            var roles = _courseItem.GetAllRoles().Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.Name
            });

            ViewBag.Emails = new SelectList(users, "Value", "Text");
            ViewBag.RolesName = new SelectList(roles, "Value", "Text");
            return View(instructorViewModel);
        }

    }
}
