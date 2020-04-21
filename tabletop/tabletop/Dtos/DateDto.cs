using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace tabletop.Dtos
{
    public class DateDto
    {

        public DateDto()
        {
            Date = "0";
        }

        public string Date { get; set; }
        public string Name { get; set; }


        public string LeadingZero(int number)
        {
            if (number <= 9)
            {
                return "0" + number;
            }

            return number.ToString();
        }

        public int GetRelativeDays(DateTime date)
        {

            var today = RoundDown(DateTime.UtcNow, new TimeSpan(1, 0,0,0));

            var differenceDateTime = today - date;

            return date.Year >= 2015 ? differenceDateTime.Days : 0;
        }

        public DateTime UtcDateTimeToAmsterdamDateTime(DateTime inputUtcDateTime)
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            // https://github.com/joeaudette/cloudscribe.SimpleContent/issues/1
            return TimeZoneInfo.ConvertTime(inputUtcDateTime, TimeZoneInfo.Utc, 
	            TimeZoneInfo.FindSystemTimeZoneById(isWindows ? "W. Europe Standard Time" : "Europe/Berlin"));
        }
        
        public DateTime AmsterdamDateTimeToUTc(DateTime inputUtcDateTime)
        {
	        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
	        return TimeZoneInfo.ConvertTime(inputUtcDateTime, 
		        TimeZoneInfo.FindSystemTimeZoneById(isWindows ? "W. Europe Standard Time" : "Europe/Berlin"), 
		        TimeZoneInfo.Utc);
        }

        public DateTime GetDateTime()
        {
            DateTime dateTime;

            var parsedBool = Int32.TryParse(Date, out var relativeDate);
            if (parsedBool)
            {

                if (relativeDate <= -1)
                {
                    relativeDate = relativeDate * -1;
                }

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
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

        public DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            var modTicks = dt.Ticks % d.Ticks;
            var delta = modTicks != 0 ? d.Ticks - modTicks : 0;
            return new DateTime(dt.Ticks + delta, dt.Kind);
        }

        public DateTime RoundDown(DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            return new DateTime(dt.Ticks - delta, dt.Kind);
        }



    }
}
