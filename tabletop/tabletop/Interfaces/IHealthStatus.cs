namespace tabletop.Interfaces
{
	public interface IHealthStatus
	{
		void Update(string channelUserId);
		void Get(string channelUserId);
	}
}
