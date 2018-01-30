using System;
using System.ComponentModel.DataAnnotations;

namespace tabletop.Models
{
    public class GetStatus
    {
        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public bool IsFree { get; set; }

        public TimeSpan Difference { get; set; }
    }
}
