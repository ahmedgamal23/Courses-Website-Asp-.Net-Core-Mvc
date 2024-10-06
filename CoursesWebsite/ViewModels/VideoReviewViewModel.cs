using CoursesWebsite.Models;

namespace CoursesWebsite.ViewModels
{
    public class VideoReviewViewModel
    {
        public Video? Video { get; set; }
        public List<Review>? Reviews { get; set; }

        public Review? NewReview { get; set; }
    }
}
