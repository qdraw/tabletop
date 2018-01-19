using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tabletop.Models;

namespace tabletop.ViewModels
{
    public class EventsOfficeHoursModel
    {
        public EventsOfficeHoursModel()
        {
            TimeZone = "+00:00";
        }
        public string TimeZone { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public int Length { get; set; }

        public DayOfWeek Day { get; set; }
        //public string DayString { get Day; Day }

        //public IEnumerable<UpdateStatus> Events { get; set; }
        //public List<EventItemModel> EventItems { get; set; }
        public List<WeightViewModel> AmountOfMotions { get; set; }
    }
}
