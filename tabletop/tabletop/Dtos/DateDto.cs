using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace tabletop.Dtos
{
    public class DateDto
    {
        public string Date { get; set; }
        public string Name { get; set; }

        public DateTime GetDateTime()
        {
            DateTime dateTime;

            var parsedBool = Int32.TryParse(Date, out var relativeDate);
            if (parsedBool)
            {
                dateTime = DateTime.UtcNow.Subtract(new TimeSpan(relativeDate, 0, 0, 0));
                dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            }
            else
            {
                DateTime.TryParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            }

            return dateTime.Year <= 2015 ? new DateTime() : dateTime;
        }

        public Int32 GetUnixTime(DateTime dateTime)
        {
            return (Int32) (dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }


        public DateTime UnixTimeToDateTime(Int32 unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }



    }
}
