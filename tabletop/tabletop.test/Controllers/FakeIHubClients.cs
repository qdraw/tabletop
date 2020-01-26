using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace tabletop.tests.Controllers
{
	public class FakeIHubClients : IHubClients<IClientProxy>, IHubClients
	{
		public IClientProxy AllExcept(IReadOnlyList<string> excludedConnectionIds)
		{
			throw new System.NotImplementedException();
		}

		public IClientProxy Client(string connectionId)
		{
			throw new System.NotImplementedException();
		}

		public IClientProxy Clients(IReadOnlyList<string> connectionIds)
		{
			throw new System.NotImplementedException();
		}

		public IClientProxy Group(string groupName)
		{
			throw new System.NotImplementedException();
		}

		public IClientProxy GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds)
		{
			throw new System.NotImplementedException();
		}

		public IClientProxy Groups(IReadOnlyList<string> groupNames)
		{
			throw new System.NotImplementedException();
		}

		public IClientProxy User(string userId)
		{
			throw new System.NotImplementedException();
		}

		public IClientProxy Users(IReadOnlyList<string> userIds)
		{
			throw new System.NotImplementedException();
		}

		public IClientProxy All { get; }
	}
}
