using CoursesWebsite.Areas.InstructorsArea.Data;
using CoursesWebsite.Data;
using CoursesWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoursesWebsite.Areas.InstructorsArea.Controllers
{
    [Area("InstructorsArea")]
    [Authorize(Roles = "Instructor")]
    public class VideoController : Controller
    {
        private IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public IActionResult Index()
        {
            /*
             *  For Text 
             *  Index Will contain (Add Video - Edit Video) as a button
             */
            var videos = _videoService.GetItems();
            return View(videos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var courses = _videoService.GetCourses().Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Title
            });
            ViewBag.Courses = new SelectList(courses, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Video video)
        {
            video.Course = _videoService.GetCourses().Where(x => x.Id == video.CourseId).FirstOrDefault()!;
            if (ModelState.IsValid)
            {
                bool result = await _videoService.Create(video);
                if(result)
                    return RedirectToAction("Index");
            }
            var courses = _videoService.GetCourses().Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Title
            });
            ViewBag.Courses = new SelectList(courses, "Value" , "Text", video.CourseId);
            return View(video);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var video = _videoService.GetItem(id);
            var courses = _videoService.GetCourses().Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Title
            });
            ViewBag.Courses = new SelectList(courses, "Value", "Text", video.CourseId);
            return View(video);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Video video)
        {
            if (ModelState.IsValid)
            {
                bool result = await _videoService.Edit(video);
                if (result)
                    return RedirectToAction("Index");
            }
            var courses = _videoService.GetCourses().Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Title
            });
            ViewBag.Courses = new SelectList(courses, "Value", "Text", video.CourseId);
            return View(video);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            bool result = _videoService.Delete(id);
            if(result) return RedirectToAction("Index");
            return NotFound();
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            var video = _videoService.GetItem(id); 
            return View(video);
        }

    }
}
