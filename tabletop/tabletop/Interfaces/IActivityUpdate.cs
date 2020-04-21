using System.Threading.Tasks;
using tabletop.Models;

namespace tabletop.Interfaces
{
	public interface IActivityUpdate
	{
		Task<bool> Add(ChannelActivity model, string nameUrlSafe);
	}
}
