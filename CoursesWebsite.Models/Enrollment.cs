using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesWebsite.Models
{
    public class Enrollment
    {
        [Key]
        public string Id {  get; set; }
        public DateTime EnrollmentDate { get; set; }

        [ForeignKey(nameof(Course))]
        public string? CourseId { get; set; }
        public Course? Course { get; set; }

        // user id
        public string? UserId { get; set; }
        public bool isEnrolled { get; set; } = false;

        public Enrollment()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
