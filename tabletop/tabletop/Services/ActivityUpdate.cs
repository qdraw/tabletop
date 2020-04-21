using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tabletop.Data;
using tabletop.Interfaces;
using tabletop.Models;

namespace tabletop.Services
{
	public class ActivityUpdate : IActivityUpdate
	{
		private readonly AppDbContext _context;

		public ActivityUpdate(AppDbContext context)
		{
			_context = context;
		}

		public async Task<bool> Add(ChannelActivity model, string nameUrlSafe)
		{

			var channelUser =
				await _context.ChannelUser.FirstOrDefaultAsync(p => 
					string.Equals(p.NameUrlSafe, nameUrlSafe, StringComparison.InvariantCultureIgnoreCase));
			if ( channelUser == null ) return false;
			model.ChannelUser = channelUser;
			
			await _context.ChannelActivity.AddAsync(model);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
