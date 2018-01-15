using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tabletop.Models
{
    public class RecentStatusClass
    {
        [Required, MaxLength(80)]
        public string Name { get; set; }

        [Required]
        public IEnumerable<UpdateStatus> RecentStatus { get; set; }

    }
}
