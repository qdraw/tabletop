using System;
using System.ComponentModel.DataAnnotations;

namespace tabletop.Models
{
    public class GetStatus
    {
        [Required]
        public DateTime DateTime { get; set; }

        public string DateTimeUtcString
        {
            get
            {
                return DateTime.Year >= 2010 ? DateTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz") : null;
            }
        }

        [Required]
        public bool IsFree { get; set; }

        public TimeSpan Difference { get; set; }
    }
}
