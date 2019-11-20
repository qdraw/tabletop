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

        private string NameIdPrivate { get; set; }

        [Key]
        [MaxLength(80)]
        [Column(Order = 1)]
        public string NameId
        {
	        get => string.IsNullOrEmpty(NameIdPrivate) ? string.Empty : NameIdPrivate;
	        set => NameIdPrivate = value;
        }

        [Required]
        [MaxLength(80)]
        [Column(Order = 2)]
        public string NameUrlSafe { get; set; }

        public bool IsVisible { get; set; }
        public bool IsAccessible { get; set; }

        [MaxLength(100)]
        public string Bearer { get; set; }

        public IEnumerable<ChannelEvent> ChannelEvents { get; set; }

    }
}
