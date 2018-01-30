using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tabletop.Models
{
    public class ChannelEvent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Status { get; set; }

        [Required, MaxLength(80)]
        public ChannelUser ChannelUser { get; set; }

        public string ChannelUserId { get; set; }


        [Column(Order = 1)]
        public DateTime DateTime { get; set; }

        public int Weight { get; set; }

    }

}
