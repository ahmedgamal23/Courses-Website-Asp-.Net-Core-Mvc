using CoursesWebsite.Data;
using CoursesWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace CoursesWebsite.Areas.InstructorsArea.Data
{
    public class VideoService : IVideoService
    {
        private readonly CoursesWebsiteDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;

        public VideoService(CoursesWebsiteDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> Create(Video item)
        {
            if (item == null || item.VideoFile == null) return false;

            string outerPath = "assets/Videos/Courses";
            string videoPath = Path.Combine(outerPath, Guid.NewGuid().ToString() + Path.GetExtension(item.VideoFile.FileName)); // save in sql
            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, videoPath);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await item.VideoFile.CopyToAsync(stream);
            }
            item.VideoPath = videoPath;
            _context.Videos.Add(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool Delete(string id)
        {
            var video = _context.Videos.Where(x => x.Id == id).FirstOrDefault();

            if(video == null) return false;

            // remove video from server
            if (System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, video.VideoPath)))
            {
                System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, video.VideoPath));
            }
            _context.Videos.Remove(video);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> Edit(Video item)
        {
            if (item == null) return false;

            var existingItem = _context.Videos.Where(x => x.Id == item.Id).FirstOrDefault();

            if(existingItem != null)
            {
                if (item.VideoFile != null)
                {
                    // update old Video
                    string outerPath = "assets/Videos/Courses";
                    string videoPath = Path.Combine(outerPath, Guid.NewGuid().ToString() + Path.GetExtension(item.VideoFile.FileName)); // save in sql
                    string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, videoPath);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await item.VideoFile.CopyToAsync(stream);
                    }
                    // remove old video
                    if (System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, existingItem.VideoPath)))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    existingItem.VideoPath = videoPath;
                }

                existingItem.Title = item.Title;
                existingItem.Description = item.Description;
                existingItem.UploadDate = item.UploadDate;
                existingItem.CourseId = item.CourseId;

                _context.Videos.Update(existingItem);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public Video GetItem(string id)
        {
            var video = _context.Videos.Where(x => x.Id == id).FirstOrDefault();
            return video!;
        }

        public IEnumerable<Video> GetItems()
        {
            return _context.Videos.Include(x=>x.Course).ToList();
        }

        public ICollection<Course> GetCourses()
        {
            return _context.Courses.ToList();
        }
    }
}
