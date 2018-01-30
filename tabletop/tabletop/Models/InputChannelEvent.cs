using System.ComponentModel.DataAnnotations;

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
