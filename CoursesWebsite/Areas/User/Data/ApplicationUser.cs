using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesWebsite.Areas.User.Data
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}

