using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tabletop.Models
{
    public class ChannelUser
    {

        [Required]
        [StringLength(80)]
        [Display(Name = "Channel")]
        public string Name { get; set; }

        [Key]
        [MaxLength(80)]
        [Column(Order = 1)]
        public string NameId { get; set; }

        [Required]
        [MaxLength(80)]
        [Column(Order = 2)]
        public string NameUrlSafe { get; set; }

        public bool IsVisible { get; set; }
        public bool IsAccessible { get; set; }


        public IEnumerable<ChannelEvent> ChannelEvents { get; set; }

    }
}
