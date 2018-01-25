using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tabletop.Models
{
    public class UpdateStatus
    {
        public int Id { get; set; }

        [Required]
        public int Status { get; set; }

        [Required, MaxLength(80)]
        public string Name { get; set; }

        //[Key]
        //[Column(Order = 1)]
        //public int NameId { get; set; }

        //[Key]
        [Column(Order = 1)]
        public DateTime DateTime { get; set; }

        public int Weight { get; set; }

        //public IEnumerable<Channel> Channels { get; set; }

    }

}
