using System;
using System.Globalization;
using tabletop.Dtos;

namespace tabletop.Models
{
	public class InputChannelActivity
	{
		public string OccurredAt { get; set; }
		public string EventName { get; set; }
		

		/// <summary>
		/// Number of milliseconds needed
		/// </summary>
		public string Value1 { get; set; }
		
		/// <summary>
		/// Description
		/// </summary>
		public string Value2 { get; set; }

		/// <summary>
		/// Is Successful
		/// </summary>
		public string Value3 { get; set; }

		/// <summary>
		/// OccurredAt => Output value
		/// </summary>
		public DateTime DateTime
		{
			get
			{
				if ( string.IsNullOrWhiteSpace(OccurredAt) )
				{
					return default;
				}

				string pattern = "MMMM d, yyyy \\a\\t hh:mmtt";
				CultureInfo provider = CultureInfo.CreateSpecificCulture("en-US");
				DateTime.TryParseExact(OccurredAt, pattern, provider,
					DateTimeStyles.AdjustToUniversal, out var itemDateTime);

				return new DateDto().AmsterdamDateTimeToUTc(itemDateTime);;
			}
		}

		public bool Success
		{
			get
			{
				if ( string.IsNullOrWhiteSpace(Value3) )
				{
					return default;
				}

				bool.TryParse(Value3, out var result);
				return result;
			}
		}

		public TimeSpan TimeSpan
		{
			get
			{
				if ( string.IsNullOrWhiteSpace(Value1)  )
				{
					return default;
				}

				int.TryParse(Value1, NumberStyles.Integer, CultureInfo.InvariantCulture,
					out var number);
				return TimeSpan.FromMilliseconds(number);
			}
		}
		
		public string Description
		{
			get
			{
				if ( string.IsNullOrWhiteSpace(Value2)  )
				{
					return default;
				}
				return Value2;
			}
		}

	}
}
