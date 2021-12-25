using System;
using tabletop.Models;

namespace tabletop.ViewModels
{
	public class LastAvailableViewModel
	{
		public DateTime LastAvailable { get; set; }

		/// <summary>
		/// Last checked dateTime
		/// </summary>
		public DateTime DateTime { get; set; }
		public bool Success { get; set; } = true;
		public string ChannelUserId { get; set; }
		public TimeSpan TimeSpan { get; set; } = new TimeSpan();
		public string Description { get; set; } = "";
		public ChannelUser ChannelUser { get; set; }
	}
}
