using System;
using System.ComponentModel.DataAnnotations;

namespace tabletop.Models
{
	public class ChannelOperations
	{
		[Required, MaxLength(80)]
		public ChannelUser ChannelUser { get; set; }

		public string ChannelUserId { get; set; }

		public DateTime Start  { get; set; }
		public DateTime End  { get; set; }

	}
}
