using CoursesWebsite.Areas.User.Data;
using CoursesWebsite.Data;
using CoursesWebsite.Models;
using CoursesWebsite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursesWebsite.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class VideoController : Controller
    {
        private readonly IServiceData<Video> _videoService;
        private readonly IServiceData<Review> _reviewService;

        public VideoController(IServiceData<Video> videoService, IServiceData<Review> reviewService)
        {
            _videoService = videoService;
            _reviewService = reviewService;
        }

        public IActionResult Index()
        {
            var videos = _videoService.GetItems();
            return View(videos);
        }

        [HttpGet]
        public IActionResult Show(string id)
        {
            var video = _videoService.GetItem(id);
            if (video == null)
                return NotFound();

            // fetch reviews for this video
            var reviews = _reviewService.GetItems().Where(x => x.VideoId == video.Id).ToList();
            var viewModel = new VideoReviewViewModel
            {
                Video = video,
                Reviews = reviews,
                NewReview = new Review()
            };
            return View(viewModel);
        }

    }
}
