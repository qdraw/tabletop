﻿using System;
using Microsoft.Extensions.Caching.Memory;
using tabletop.Data;
using tabletop.Interfaces;
using tabletop.Models;

namespace tabletop.Services
{
	public class HealthStatus : IHealthStatus
	{
		private readonly AppDbContext _context;
		private readonly IMemoryCache _cache;

		public HealthStatus(AppDbContext context, IMemoryCache cache)
		{
			_context = context;
			_cache = cache;
		}
		private string CacheIsFreeName(string channelUserId)
		{
			return "HealthStatus_" + channelUserId;
		}
		
		public void Update(string nameUrlSafe)
		{

			var channelUser = new SqlUpdateStatus(_context, _cache)
				.GetChannelUserIdByUrlSafeName(nameUrlSafe, true);
			
			if(channelUser == null) return;
			
			var queryCacheName = CacheIsFreeName(channelUser.NameId);
					
			_cache.Remove(queryCacheName);
			_cache.Set(queryCacheName, DateTime.UtcNow); // no timeout, or delete after timespan
			
		}

		public DateTime Get(string nameUrlSafe)
		{
			var channelUser = new SqlUpdateStatus(_context, _cache)
				.GetChannelUserIdByUrlSafeName(nameUrlSafe, true);
			
			if(channelUser == null) return new DateTime();
			
			var queryCacheName = CacheIsFreeName(channelUser.NameId);
			if ( _cache.TryGetValue(queryCacheName, out var latestEventObject) )
			{
				return ( DateTime ) latestEventObject;
			}

			return new DateTime();

		}
	}
}
