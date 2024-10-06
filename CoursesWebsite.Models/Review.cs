using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesWebsite.Models
{
    public class Review
    {
        [Key]
        public string Id {  get; set; }

        [Range(1,5)]
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; }

        [ForeignKey(nameof(Video))]
        public string? VideoId { get; set; }
        public Video? Video { get; set; }

        public Review()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
