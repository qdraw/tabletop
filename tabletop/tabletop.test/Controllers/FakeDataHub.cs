using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using tabletop.Hubs;

namespace tabletop.tests.Controllers
{
	public class FakeDataHub : IHubContext<DataHub>
	{
		public FakeDataHub(IHubClients input, IGroupManager groupManager)
		{
			Clients = input;
			Groups = groupManager;
		}
		public IHubClients Clients { get; }
		public IGroupManager Groups { get; }
	}
}
