using System;

namespace tabletop.ViewModels
{
    public class WeightViewModel
    {
        public int Weight { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public string Label { get; set; }
        public string LabelUtc { get; set; }
    }
}
