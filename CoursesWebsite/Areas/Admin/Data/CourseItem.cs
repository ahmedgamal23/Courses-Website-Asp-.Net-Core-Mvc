using CoursesWebsite.Areas.User.Data;
using CoursesWebsite.Data;
using CoursesWebsite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace CoursesWebsite.Areas.Admin.Data
{
    public class CourseItem : ICoursesItems
    {
        private readonly CoursesWebsiteDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CourseItem(CoursesWebsiteDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IEnumerable<Course> GetItems()
        {
            var courses = _context.Courses.Include(x => x.Category).ToList();
            return courses;
        }

        public Course GetItem(string id)
        {
            var course = _context.Courses.Where(x => x.Id == id).FirstOrDefault();
            return course!;
        }

        public async Task<bool> Create(Course course)
        {
            if (course != null)
            {
                if (course.ImagePath == null)
                    return false;
                // add image to server
                var outerPath = "assets/images/admin/course";
                var imgPath = Guid.NewGuid().ToString() + Path.GetExtension(course.ImageFile.FileName);
                imgPath = Path.Combine(outerPath, imgPath);
                var fullPath = Path.Combine(_environment.WebRootPath, imgPath);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await course.ImageFile.CopyToAsync(stream);
                }
                course.Category = _context.Categories.Where(x => x.Id == course.CategoryId).FirstOrDefault() ?? new Category();
                course.ImagePath = imgPath;
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Edit(Course course)
        {
            if (course != null)
            {
                // Retrieve the existing category from the database
                var existingCourse = await _context.Courses.FindAsync(course.Id);
                if (existingCourse == null)
                {
                    return false;
                }

                string imgCopyPath = existingCourse.ImagePath;

                if (course.ImageFile != null)
                {
                    // Remove old image from server
                    if (!string.IsNullOrEmpty(existingCourse.ImagePath))
                    {
                        var oldImagePath = Path.Combine(_environment.WebRootPath, existingCourse.ImagePath);
                        if (File.Exists(oldImagePath))
                        {
                            File.Delete(oldImagePath);
                        }
                    }

                    // Add new image to server
                    var outerPath = "assets/images/admin/course";
                    var imgPath = Guid.NewGuid().ToString() + Path.GetExtension(course.ImageFile.FileName);
                    imgPath = Path.Combine(outerPath, imgPath);
                    var fullPath = Path.Combine(_environment.WebRootPath, imgPath);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await course.ImageFile.CopyToAsync(stream);
                    }
                    imgCopyPath = imgPath;
                }

                // Update course properties
                existingCourse.Title = course.Title;
                existingCourse.Description = course.Description;
                existingCourse.ImagePath = imgCopyPath;
                existingCourse.Duration = course.Duration;
                existingCourse.UpdatedDate = course.UpdatedDate;
                existingCourse.CreatedDate = course.CreatedDate;
                existingCourse.CategoryId = course.CategoryId;
                existingCourse.Category = course.Category;
                existingCourse.StudentCount = existingCourse.StudentCount + course.StudentCount;

                _context.Courses.Update(existingCourse);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool Delete(string id)
        {
            var course = _context.Courses.Where(x => x.Id == id).FirstOrDefault();
            if (course == null)
                return false;
            // remove image from server
            if (!string.IsNullOrEmpty(course.ImagePath))
            {
                var oldImagePath = Path.Combine(_environment.WebRootPath, course.ImagePath);
                if (File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Category> GetCategories()
        {
            // Fetch all categories from the database
            var categories = _context.Categories.ToList();
            return categories;
        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public IEnumerable<IdentityRole> GetAllRoles()
        {
            return _context.Roles.ToList();
        }
    }
}

