using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Rest.Azure;
using tabletop.Data;
using tabletop.Interfaces;
using tabletop.Models;

namespace tabletop.Services
{
    public class SqlUpdateStatus : IUpdate
    {
        private readonly AppDbContext _context;

        public SqlUpdateStatus(AppDbContext context)
        {
            _context = context;
        }
       
        public IEnumerable<ChannelEvent> GetTimeSpanByName(string urlsafename, DateTime startDateTime, DateTime endDateTime)
        {
            var result = _context.ChannelEvent
                    .Where(p => p.DateTime >= startDateTime && p.DateTime <= endDateTime)
                    .Where(b => b.ChannelUser.NameUrlSafe == urlsafename);
            return result;
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
                    return newStatusContent;
                }

                if (firstLastMinuteContent == null)
                    throw new CloudException("", new Exception("request from database went wrong"));

                firstLastMinuteContent.Weight++;
                firstLastMinuteContent.DateTime = DateTime.UtcNow;
                _context.Attach(firstLastMinuteContent).State = EntityState.Modified;
                _context.SaveChanges();
                return firstLastMinuteContent;
            }
            else
            {
                throw new NotImplementedException("",new Exception("fail"));
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
            var userIdObjectIsAccessible = _context.ChannelUser.LastOrDefault(p => p.NameUrlSafe == nameUrlSafe);

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
            return _context.ChannelUser; // return null if not there
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


        public GetStatus IsFree(string channelUserId)
        {
            var latestEvent = _context.ChannelEvent
                .LastOrDefault(b => b.ChannelUserId == channelUserId);

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
                DateTime = latestEvent.DateTime,
                Difference = difference
            };

            if (difference.Minutes > 2)
            {
                isFreeStatus.IsFree = true;
            }

            return isFreeStatus;
        }
        //public GetStatus IsFree(string nameUrlSafe)
        //{
        //    var latestEvent = _context.ChannelEvent
        //        .Where(b => b.ChannelUser.NameUrlSafe == nameUrlSafe)
        //        .Include(b => b.ChannelUser)
        //        .LastOrDefault();

        //    if (latestEvent == null)
        //    {
        //        return new GetStatus
        //        {
        //            IsFree = true
        //        };
        //    }

        //    var difference = DateTime.UtcNow - latestEvent.DateTime;

        //    var isFreeStatus = new GetStatus
        //    {
        //        Name = latestEvent.ChannelUser.Name,
        //        DateTime = latestEvent.DateTime,
        //        Difference = difference
        //    };

        //    if (difference.Minutes > 2)
        //    {
        //        isFreeStatus.IsFree = true;
        //    }

        //    return isFreeStatus;
        //}



        public string G()
        {
            return "";
            //var startDateTime = dateTime.ToUniversalTime().AddHours(9);
            //var endDateTime = dateTime.ToUniversalTime().AddHours(18);
            //var statusContent = _updateStatusContent.GetTimeSpanByName(
            //        dto.Name,
            //        startDateTime,
            //        endDateTime
            //    ).ToList().OrderBy(p => p.DateTime);

            //var model = new EventsOfficeHoursModel
            //{
            //    Day = dateTime.DayOfWeek,
            //    StartDateTime = startDateTime,
            //    EndDateTime = endDateTime,
            //    AmountOfMotions = new List<WeightViewModel>()
            //};

            //var i = dto.GetUnixTime(startDateTime);
            //while (i <= dto.GetUnixTime(endDateTime))
            //{

            //    var startIntervalDateTime = dto.UnixTimeToDateTime(i);
            //    var endIntervalDateTime = dto.UnixTimeToDateTime(i + interval);

            //    var contentUpdateStatuses = statusContent.Where(p => p.DateTime > startIntervalDateTime && p.DateTime < endIntervalDateTime).ToList();
            //    // Sum Weight Select-Statement >> Advies Joost!
            //    //var contentUpdateStatusesExtended = statusContent
            //    //    .Where(p => p.DateTime > startIntervalDateTime && p.DateTime < endIntervalDateTime)
            //    //    .GroupBy(p => p.Weight)
            //    //    .Select(p => new { WeightSum = p.Sum(c => c.Weight) }); // Sum Weight Select-Statement

            //    var eventItem = new WeightViewModel();
            //    eventItem.StartDateTime = startIntervalDateTime;
            //    eventItem.EndDateTime = endIntervalDateTime;

            //    //if (contentUpdateStatusesExtended != null && contentUpdateStatusesExtended.Any() && contentUpdateStatusesExtended.FirstOrDefault() != null)
            //    //{
            //    //    eventItem.Weight = contentUpdateStatusesExtended.FirstOrDefault().WeightSum;

            //    //}

            //    eventItem.Weight = 0;
            //    eventItem.Label = startIntervalDateTime.ToString("HH:mm");

            //    if (contentUpdateStatuses.Any())
            //    {
            //        foreach (var item in contentUpdateStatuses)
            //        {
            //            eventItem.Weight += item.Weight;
            //        }
            //    }

            //    model.AmountOfMotions.Add(eventItem);

            //    i += interval;
            //}

            //model.Length = model.AmountOfMotions.Count();

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
}