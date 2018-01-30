using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tabletop.Dtos;

namespace tabletop.tests
{
    [TestClass]
    public class DateDtoTest
    {
        public DateTime RoundDown(DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            return new DateTime(dt.Ticks - delta, dt.Kind);
        }

        [TestMethod]
        public void GetDateTimeAbsoluteToday()
        {
            var model = new DateDto();

            var today = RoundDown(DateTime.UtcNow, new TimeSpan(1, 0, 0, 0));

            model.Date = today.ToString("yyyy-MM-dd");
            var getDateTimeFromModel = model.GetDateTime();

            Assert.AreEqual(getDateTimeFromModel, today);
        }

        [TestMethod]
        public void GetDateTimeRelativeYesterday()
        {
            var model = new DateDto();

            var yesterday = RoundDown(DateTime.UtcNow, new TimeSpan(1, 0, 0, 0)).AddDays(-1);

            model.Date = "-1";
            var getDateTimeFromModel = model.GetDateTime();

            Assert.AreEqual(getDateTimeFromModel, yesterday);
        }
    }
}
