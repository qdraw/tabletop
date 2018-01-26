using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tabletop.Models
{
    public class ChannelEventsModel
    {
        [Required, MaxLength(80)]
        public string Name { get; set; }

        [Required]
        public IEnumerable<ChannelEvent> Events { get; set; }

    }
}
