using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tabletop.Models
{
    public class InputChannelEvent
    {
        [Required]
        public int Status { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
