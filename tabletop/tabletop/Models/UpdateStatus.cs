using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace tabletop.Models
{
    public class UpdateStatus
    {
        public int Id { get; set; }
        //[Display(Name = "Restaurant Name")]
        //[Required, MaxLength(80)]
        [Required]
        public int Status { get; set; }

        [Required, MaxLength(80)]
        public string Name { get; set; }

        public DateTime DateTime { get; set; }

        public int Weight { get; set; }
    }
}
