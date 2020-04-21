using System;
using System.ComponentModel.DataAnnotations;

namespace tabletop.Models
{
	public class ChannelActivity
	{
		[Key]
		public int Id { get; set; }

		[Required, MaxLength(80)]
		public ChannelUser ChannelUser { get; set; }
		
		public string ChannelUserId { get; set; }

		public DateTime DateTime { get; set; }
			
		[Required] 
		public bool Success { get; set; } = true;

		public TimeSpan TimeSpan { get; set; } = new TimeSpan();

		[MaxLength(1000000)]
		public string Description { get; set; } = "";
	}
}
