using CoursesWebsite.Areas.Identity.Data;
using CoursesWebsite.Areas.User.Data;
using CoursesWebsite.Data;
using CoursesWebsite.Models;
using CoursesWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CoursesWebsite.Areas.User.Controllers
{
    [Area("User")]
    public class ReviewController : Controller
    {
        private readonly IServiceData<Review> _userService;

        public ReviewController(IServiceData<Review> userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VideoReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                var review = model.NewReview;
                review!.Id = Guid.NewGuid().ToString();
                review.ReviewDate = DateTime.Now;

                // Save the new review
                bool result = await _userService.Create(review);

                return RedirectToAction("Show", "Video", new { id = review.VideoId });
            }
            return NotFound();
        }


    }
}
