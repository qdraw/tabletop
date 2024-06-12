using System.Collections.Generic;
using System.Threading.Tasks;
using tabletop.Models;
using tabletop.ViewModels;

namespace tabletop.Interfaces
{
	public interface IActivityUpdate
	{
		Task<bool> Add(ChannelActivity model, string nameUrlSafe);

		List<LastAvailableViewModel> IsAvailable();
	}
}
