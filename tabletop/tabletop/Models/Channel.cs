using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tabletop.Models
{
    public class Channel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        [Display(Name = "Channel Name")]
        public string Name { get; set; }

        public string NameId { get; set; }

        public bool IsVisible { get; set; }

        //public UpdateStatus UpdateStatus { get; set; }
    }
}
