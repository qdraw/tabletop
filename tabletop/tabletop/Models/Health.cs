using System;

namespace tabletop.Models
{
	public class Health
	{
		public DateTime StartupDateTime { get; set; }
		public DateTime LastPing { get; set; }
		public TimeSpan Difference { get; set; }
	}
}
