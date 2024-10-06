using CoursesWebsite.Data;
using CoursesWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursesWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {       
        private IServiceData<Category> _categoryItem;

        public CategoryController(IServiceData<Category> categoryItem)
        {
            _categoryItem = categoryItem;
        }

        public IActionResult Index()
        {
            var category = _categoryItem.GetItems();
            return View(category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                bool result = await _categoryItem.Create(category);
                if (result)
                {
                    // category added successfully.
                    return RedirectToAction("Index", "Category");
                }
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (ModelState.IsValid)
            {
                var category = _categoryItem.GetItem(id);
                if(category != null)
                {
                    return View(category);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryItem.Edit(category);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var result = _categoryItem.Delete(id);
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
