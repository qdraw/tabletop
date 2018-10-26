using System;

namespace tabletop.Interfaces
{
	public interface IHealthStatus
	{
		void Update(string channelUserId);
		DateTime Get(string channelUserId);
	}
}
