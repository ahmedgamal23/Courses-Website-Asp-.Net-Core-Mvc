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
    public class Course
    {
        [Key]
        public string Id { get; set; }

        public Course()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string? InstructorId { get; set; }
        public int StudentCount { get; set; }
        public TimeOnly Duration { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<Video>? Videos { get; set; }

        [ForeignKey(nameof(Category))]
        public string? CategoryId { get; set; }
        public Category? Category { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
