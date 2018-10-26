using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

	    [NotMapped]
	    public bool IsHealthPingEnabled { get; set; } = true;

        [MaxLength(100)]
        public string Bearer { get; set; }

        public IEnumerable<ChannelEvent> ChannelEvents { get; set; }
	    public IEnumerable<ChannelOperations> ChannelOperations { get; set; }

    }
}
