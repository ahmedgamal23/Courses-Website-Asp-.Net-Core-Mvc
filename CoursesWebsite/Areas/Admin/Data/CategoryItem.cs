using CoursesWebsite.Data;
using CoursesWebsite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesWebsite.Areas.Admin.Data
{
    internal class CategoryItem : IServiceData<Category>
    {
        private readonly CoursesWebsiteDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CategoryItem(CoursesWebsiteDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> Create(Category category)
        {
            if (category != null)
            {
                if (category.ImagePath == null)
                    return false;
                // add image to server
                var outerPath = "assets/images/admin/category";
                var imgPath = Guid.NewGuid().ToString() + Path.GetExtension(category.ImageFile!.FileName);
                imgPath = Path.Combine(outerPath, imgPath);
                var fullPath = Path.Combine(webHostEnvironment.WebRootPath, imgPath);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await category.ImageFile.CopyToAsync(stream);
                }
                category.ImagePath = imgPath;
                _context.Categories.Add(category);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public IEnumerable<Category> GetItems()
        {
            var categories = _context.Categories.ToList();
            return categories;
        }

        public Category GetItem(string id)
        {
            var category = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
            return category!;
        }

        public async Task<bool> Edit(Category category)
        {
            if (category != null)
            {
                // Retrieve the existing category from the database
                var existingCategory = await _context.Categories.FindAsync(category.Id);
                if (existingCategory == null)
                {
                    return false;
                }

                string imgCopyPath = existingCategory.ImagePath;

                if (category.ImageFile != null)
                {
                    // Remove old image from server
                    if (!string.IsNullOrEmpty(existingCategory.ImagePath))
                    {
                        var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, existingCategory.ImagePath);
                        if (File.Exists(oldImagePath))
                        {
                            File.Delete(oldImagePath);
                        }
                    }

                    // Add new image to server
                    var outerPath = "assets/images/admin/category";
                    var imgPath = Guid.NewGuid().ToString() + Path.GetExtension(category.ImageFile.FileName);
                    imgPath = Path.Combine(outerPath, imgPath);
                    var fullPath = Path.Combine(webHostEnvironment.WebRootPath, imgPath);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await category.ImageFile.CopyToAsync(stream);
                    }
                    imgCopyPath = imgPath;
                }

                // Update category properties
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                existingCategory.ImagePath = imgCopyPath;

                _context.Categories.Update(existingCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool Delete(string id)
        {
            var category = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
            if (category == null)
                return false;
            // remove image from server
            if (!string.IsNullOrEmpty(category.ImagePath))
            {
                var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, category.ImagePath);
                if (File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return true;
        }
    }
}
