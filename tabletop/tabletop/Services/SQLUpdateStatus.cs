using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using tabletop.Data;
using tabletop.Dtos;
using tabletop.Interfaces;
using tabletop.Models;
using tabletop.ViewModels;

namespace tabletop.Services
{
    public class SqlUpdateStatus : IUpdate
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public SqlUpdateStatus(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public IEnumerable<ChannelEvent> GetTimeSpanByName(string urlsafename, DateTime startDateTime,
            DateTime endDateTime)
        {
            var result = _context.ChannelEvent
                .Where(p => p.DateTime >= startDateTime && p.DateTime <= endDateTime)
                .Where(b => b.ChannelUser.NameUrlSafe == urlsafename);
            return result.AsEnumerable();
        }


        public ChannelUser Add(ChannelUser updateStatusContent)
        {
            _context.ChannelUser.Add(updateStatusContent);
            _context.SaveChanges();
            return updateStatusContent;
        }

        // used for unit tests
        public ChannelUser Update(ChannelUser updateUser)
        {
            _context.Attach(updateUser).State = EntityState.Modified;
            _context.SaveChanges();
            return updateUser;
        }

        // used for unit tests
        public ChannelEvent Update(ChannelEvent updateEvent)
        {
            _context.Attach(updateEvent).State = EntityState.Modified;
            _context.SaveChanges();
            return updateEvent;
        }


        public ChannelEvent AddOrUpdate(InputChannelEvent inputChannelEventContent)
        {

            if (IsUserInDatabase(inputChannelEventContent.Name))
            {

                var channelUserId = GetChannelUserIdByUrlSafeName(inputChannelEventContent.Name, true);
                var userid = channelUserId.NameId;
                var getLastMinuteContent = GetLastMinute(userid);
                var lastMinuteContent = getLastMinuteContent.ToList();
                var firstLastMinuteContent = lastMinuteContent.FirstOrDefault();


		            
                if (!lastMinuteContent.Any())
                {
                    var newStatusContent = new ChannelEvent
                    {
                        ChannelUserId = userid,
                        Status = inputChannelEventContent.Status,
                        DateTime = DateTime.UtcNow,
                        Weight = 1
                    };
                    _context.ChannelEvent.Add(newStatusContent);
                    _context.SaveChanges();
	                
	                // update isfree cache
	                CacheIsFreeUpdateItem(channelUserId.NameId, newStatusContent);
		                
                    return newStatusContent;
                }

                if (firstLastMinuteContent == null)
                    throw new ApplicationException("", new Exception("request from database went wrong"));

                firstLastMinuteContent.Weight++;
                firstLastMinuteContent.DateTime = DateTime.UtcNow;
                _context.Attach(firstLastMinuteContent).State = EntityState.Modified;
                _context.SaveChanges();
	            
	            // update isfree cache
	            CacheIsFreeUpdateItem(channelUserId.NameId, firstLastMinuteContent);
	            
                return firstLastMinuteContent;
            }
            else
            {
                throw new NotImplementedException("", new Exception("fail"));
            }
        }

	    

	    public bool IsUserInDatabase(string nameUrlSafe)
        {
            var count = _context.ChannelUser.Count(r => r.NameUrlSafe == nameUrlSafe);
            switch (count)
            {
                case 0:
                    return false;
                default:
                    return true;
            }
        }


        public ChannelUser GetChannelUserIdByUrlSafeName(string nameUrlSafe, bool internalRequest)
        {
	        var userIdObjectIsAcces1sible = _context.ChannelUser.ToList();
	        
            var userIdObjectIsAccessible = _context.ChannelUser.OrderByDescending(p => p.NameId)
	            .FirstOrDefault(p => p.NameUrlSafe == nameUrlSafe);
            
            if (userIdObjectIsAccessible == null) return null;

            if (!internalRequest)
            {
                if (userIdObjectIsAccessible.IsAccessible)
                {
                    return userIdObjectIsAccessible; // return null if not there
                }
                else
                {
                    return null;
                }
            }

            return userIdObjectIsAccessible;
        }


        public IEnumerable<ChannelUser> GetAllChannelUsers()
        {
            // Cached for a long time
            // To update a user, please query a sql and restart the application
            
            const string queryCacheName = "GetAllChannelUsers";
            if (_cache.TryGetValue(queryCacheName, out var channelUserObject))  
                return channelUserObject as IEnumerable<ChannelUser>;

            var channelUserList = (IEnumerable<ChannelUser>) _context.ChannelUser.ToList();
            _cache.Set(queryCacheName, channelUserList);
            return channelUserList; // return null if not there
        }

        public ChannelUser AddUser(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            var nameUrlSafe = name.ToLower();
            nameUrlSafe = Regex.Replace(nameUrlSafe, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            var newChannelUser = new ChannelUser
            {
                Name = name,
                IsVisible = true,
                IsAccessible = true,
                NameUrlSafe = nameUrlSafe
            };

            _context.ChannelUser.Add(newChannelUser);
            _context.SaveChanges();

            return newChannelUser;
        }

        public IEnumerable<ChannelEvent> GetLastMinute(string channelUserId)
        {
            // round minute to 18:48 for example
            var now = DateTime.UtcNow;
            var nowTicks = now.Ticks;

            var lastMinute = new DateTime(nowTicks - (nowTicks % (1000 * 1000 * 10 * 60))); // 60

            var lastMinuteRequests = _context.ChannelEvent
                .Where(p => p.DateTime > lastMinute)
                .Where(b => b.ChannelUserId == channelUserId);

            return lastMinuteRequests;
        }

        /// <summary>
        /// Cached query to check if the channel is free
        /// </summary>
        /// <param name="channelUserId">a guid</param>
        /// <returns>GetStatus object</returns>
        public GetStatus IsFree(string channelUserId)
        {
            var queryCacheName = cacheIsFreeName(channelUserId);
            if (_cache.TryGetValue(queryCacheName, out var latestEventObject))  
                return IsFree(latestEventObject as ChannelEvent);

            var isFreeStatus = IsFreeQuery(channelUserId);
            _cache.Set(queryCacheName, isFreeStatus);
            return IsFree(isFreeStatus); // return null if not there
        }

	    private string cacheIsFreeName(string channelUserId)
	    {
		    return "IsFree_latestEventObject_" + channelUserId;
	    }
	    
	    public void CacheIsFreeUpdateItem(string channelUserId, ChannelEvent latestChannelEvent)
	    {
		    if( _cache == null) return;
		    var queryCacheName = cacheIsFreeName(channelUserId);

		    if (!_cache.TryGetValue(queryCacheName, out var _)) return;
		    _cache.Remove(queryCacheName);
		    _cache.Set(queryCacheName, latestChannelEvent); // no timeout
	    }

	    private ChannelEvent IsFreeQuery(string channelUserId)
	    {
		    var latestEvent = _context.ChannelEvent
			    .OrderByDescending(p => p.Id)
			    .FirstOrDefault(b => b.ChannelUserId == channelUserId);
		    return latestEvent;
	    }

	    private GetStatus IsFree(ChannelEvent latestEvent)
        {
            if (latestEvent == null)
            {
                return new GetStatus
                {
                    IsFree = true
                };
            }

            var difference = DateTime.UtcNow - latestEvent.DateTime;

            var isFreeStatus = new GetStatus
            {
                DateTime = DateTime.SpecifyKind(latestEvent.DateTime, DateTimeKind.Utc),
                Difference = difference
            };

            var differentTimeSpan = (new TimeSpan(0, 2, 0)); // 2 minutes
            if (difference >= differentTimeSpan) 
            {
                isFreeStatus.IsFree = true;
            }

            return isFreeStatus;
        }

        public EventsOfficeHoursModel EventsRecent(string urlSafeName)
        {
            var dto = new DateDto();

            var startDateTime =
                dto.RoundDown(
                    DateTime.UtcNow.Subtract(new TimeSpan(0, 8, 0, 0)),
                    new TimeSpan(0, 0, 5, 0));

            var endDateTime = dto.RoundUp(DateTime.UtcNow, new TimeSpan(0, 0, 5, 0));

            var channelUser = GetChannelUserIdByUrlSafeName(urlSafeName, true);
            if (channelUser == null)
            {
                return null;
            }
            var channelUserId = channelUser.NameId;

            var channelEvents = _context.ChannelEvent
                .Where(
                    p => p.ChannelUserId == channelUserId &&
                         p.DateTime > startDateTime &&
                         p.DateTime < endDateTime
                ).OrderBy(p => p.DateTime);

            return ParseEvents(channelEvents.ToList(), startDateTime, endDateTime);
        }

        public EventsOfficeHoursModel EventsDayView(DateTime dateTime, string urlSafeName)
        {
            var dto = new DateDto();

            var startDateTime = dateTime;
            var endDateTime = dateTime.AddHours(24);

            var channelUser = GetChannelUserIdByUrlSafeName(urlSafeName, true);
            if (channelUser == null)
            {
                return null;
            }

            var channelUserId = channelUser.NameId;

            var channelEvents = _context.ChannelEvent
                .Where(
                    p => p.ChannelUserId == channelUserId &&
                         p.DateTime > startDateTime &&
                         p.DateTime < endDateTime
                ).OrderBy(x => x.DateTime).ToList();

            if (channelEvents.Count == 0)
            {
                var startEmthyDateTime =
                    dto.RoundDown(
                        DateTime.UtcNow.Subtract(new TimeSpan(0, 8, 0, 0)),
                        new TimeSpan(0, 0, 5, 0));

                var endEmthyDateTime = dto.RoundUp(DateTime.UtcNow, new TimeSpan(0, 0, 5, 0));

                var model = new EventsOfficeHoursModel
                {
                    Day = startDateTime.DayOfWeek,
                    StartDateTime = startEmthyDateTime,
                    EndDateTime = endEmthyDateTime,
                    AmountOfMotions = new List<WeightViewModel>(),
                    Length = 0
                };
                return model;
            }

            var median = channelEvents.Skip(channelEvents.Count() / 2).First().DateTime;
            startDateTime = dto.RoundDown(median.ToUniversalTime().AddHours(-4), new TimeSpan(0, 0, 5, 0));
            endDateTime = dto.RoundUp(median.ToUniversalTime().AddHours(4), new TimeSpan(0, 0, 5, 0));

            var channelParseEvents = channelEvents
                .Where(p => p.DateTime > startDateTime &&
                            p.DateTime < endDateTime);

            return ParseEvents(channelParseEvents.ToList(), startDateTime, endDateTime);

        }

        public EventsOfficeHoursModel ParseEvents(List<ChannelEvent> channelEvents, DateTime startDateTime, DateTime endDateTime)
        {
            var dto = new DateDto();

            var model = new EventsOfficeHoursModel
            {
                Day = startDateTime.DayOfWeek,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime,
                AmountOfMotions = new List<WeightViewModel>(),
                Length = 0
            };

            model.Length = _context.ChannelEvent
                .Count(
                    p => p.DateTime > model.StartDateTime &&
                         p.DateTime < model.EndDateTime
                );

            const int interval = 60 * 5; // 5 minutes
            var i = dto.GetUnixTime(startDateTime);
            while (i <= dto.GetUnixTime(endDateTime))
            {

                var eventItem = new WeightViewModel
                {
                    StartDateTime = dto.UnixTimeToDateTime(i),
                    EndDateTime = dto.UnixTimeToDateTime(i + interval)
                };
                eventItem.LabelUtc = eventItem.StartDateTime.ToString("HH:mm");
                eventItem.Label = dto.UtcDateTimeToAmsterdamDateTime(eventItem.StartDateTime).ToString("HH:mm");

                var weightSum = channelEvents
                    .Where(p =>
                        p.DateTime > eventItem.StartDateTime &&
                        p.DateTime < eventItem.EndDateTime)
                    .Select(p => p.Weight).Sum();

                eventItem.Weight = weightSum;
                model.AmountOfMotions.Add(eventItem);
                i += interval;
            }
            return model;

        }



    }



        


        //public UpdateStatus Get(int id)
        //{
        //    return _context.UpdateStatus.FirstOrDefault(r => r.Id == id);
        //}

        //public IEnumerable<UpdateStatus> GetAll()
        //{
        //    return _context.UpdateStatus.OrderBy(r => r.Id);
        //}

        //public IEnumerable<UpdateStatus> GetAllByName(string name)
        //{
        //    return _context.UpdateStatus.Where(b => b.Name == name);
        //}


}
