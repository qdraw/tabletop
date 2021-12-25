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
		public string TimeString { get; set; }
		
		/// <summary>
		/// Description
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Is Successful
		/// </summary>
		public string SuccessString { get; set; }

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
				if ( string.IsNullOrWhiteSpace(SuccessString) )
				{
					return default;
				}

				bool.TryParse(SuccessString, out var result);
				return result;
			}
		}

		public TimeSpan TimeSpan
		{
			get
			{
				if ( string.IsNullOrWhiteSpace(TimeString)  )
				{
					return default;
				}

				int.TryParse(TimeString, NumberStyles.Any, CultureInfo.InvariantCulture,
					out var number);
				return TimeSpan.FromMilliseconds(number);
			}
		}

	}
}
