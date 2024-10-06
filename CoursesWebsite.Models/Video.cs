using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesWebsite.Models
{
    public class Video
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public string VideoPath { get; set; } = string.Empty;
        
        [ForeignKey(nameof(Course))]
        public string? CourseId { get; set; }
        public Course? Course { get; set; }

        [NotMapped]
        public IFormFile? VideoFile { get; set; }

        public Video()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
