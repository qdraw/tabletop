using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tabletop.Data;
using tabletop.Dtos;
using tabletop.Interfaces;
using tabletop.Models;
using tabletop.ViewModels;

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

		public List<LastAvailableViewModel> IsAvailable()
		{
			var visibleChannelUsers = _context.ChannelUser.Where(p => p.IsVisible).ToList();
			
			var parsedRecentChannelActivities = new List<LastAvailableViewModel>();

			foreach ( var channelUsers in visibleChannelUsers )
			{
				var result = _context.ChannelActivity.OrderByDescending(t => t.DateTime).
					Where(p => p.ChannelUserId == channelUsers.NameId).
					Take(20).ToList();

				var lastResult = result.FirstOrDefault();
				if ( lastResult == null )
				{
					continue;
				}
				lastResult.ChannelUser.ChannelEvents = null;
				lastResult.ChannelUser.ChannelActivities = null;
				lastResult.ChannelUser.Bearer = null;

				var lastAvailable = result.FirstOrDefault(p => p.Success)?.DateTime;
				// longer than 20 times == null

				parsedRecentChannelActivities.Add(new LastAvailableViewModel
				{
					LastAvailable = new DateDto().UtcDateTimeToAmsterdamDateTime(lastAvailable),
					DateTime = new DateDto().UtcDateTimeToAmsterdamDateTime(lastResult.DateTime),
					Success = lastResult.Success,
					ChannelUserId = lastResult.ChannelUserId,
					ChannelUser = lastResult.ChannelUser,
					TimeSpan = lastResult.TimeSpan,
					Description = lastResult.Description,
				});
			}

			return parsedRecentChannelActivities.OrderBy(p => p.ChannelUser.Name).ToList();

		}
	}
}
