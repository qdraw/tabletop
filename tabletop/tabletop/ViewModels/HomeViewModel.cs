using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tabletop.Models;

namespace tabletop.ViewModels
{
    public class HomeViewModel
    {
        [MaxLength(80)]
        public string Name { get; set; }

        public IEnumerable<string> List { get; set; }

        public int RelativeDate { get; set; }
        public int TomorrowRelativeDate { get; set; }
        public int YesterdayRelativeDate { get; set; }
        public bool IsFree { get; set; }
        public DateTime IsFreeDateTime { get; set; }
        public DateTime Day { get; set; }
        public string Yesterday { get; set; }
        public string Tomorrow { get; set; }
        public string Today { get; set; }

    }
}
