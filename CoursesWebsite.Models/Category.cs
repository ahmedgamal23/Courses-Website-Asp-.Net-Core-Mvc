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
    public class Category
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public IEnumerable<Course> Courses { get; set; } = new List<Course>();

        public Category()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
