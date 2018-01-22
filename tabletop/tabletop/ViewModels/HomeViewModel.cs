using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    }
}
