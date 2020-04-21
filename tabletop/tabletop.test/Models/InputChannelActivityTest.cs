using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tabletop.Dtos;
using tabletop.Models;

namespace tabletop.tests.Models
{
	[TestClass]
	public class InputChannelActivityTest
	{
		[TestMethod]
		public void InputChannelActivityTest_21April()
		{
			var model = new InputChannelActivity {OccurredAt = "April 21, 2020 at 10:45AM"};

			var dateTime = new DateTime(2020, 04, 21, 10, 45,00);
			Assert.AreEqual(new DateDto().AmsterdamDateTimeToUTc(dateTime),model.DateTime);
		}
		
		[TestMethod]
		public void InputChannelActivityTest_2April()
		{
			var model = new InputChannelActivity {OccurredAt = "April 2, 2020 at 02:15PM"};

			var dateTime = new DateTime(2020, 04, 2, 14, 15,00);
			Assert.AreEqual(new DateDto().AmsterdamDateTimeToUTc(dateTime),model.DateTime);
		}
		
		[TestMethod]
		public void InputChannelActivityTest_True()
		{
			var model = new InputChannelActivity {Value3 = "true"};
			Assert.AreEqual(true,model.Success);
		}
		
		[TestMethod]
		public void InputChannelActivityTest_False()
		{
			var model = new InputChannelActivity {Value3 = "False"};
			Assert.AreEqual(false,model.Success);
		}

		[TestMethod]
		public void InputChannelActivityTest_TimeSpan()
		{
			var model = new InputChannelActivity{Value1 = "300"};
			Assert.AreEqual(TimeSpan.FromMilliseconds(300),model.TimeSpan);
		}
		
	}
}
