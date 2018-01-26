using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using Microsoft.Rest.Azure;
using tabletop.Data;
using tabletop.Interfaces;
using tabletop.Models;
using tabletop.ViewModels;

namespace tabletop.Services
{
    public class SqlUpdateStatus : IUpdate
    {
        private readonly AppDbContext _context;

        public SqlUpdateStatus(AppDbContext context)
        {
            _context = context;
        }


        public IEnumerable<string> GetUniqueNames()
        {
            //var allEvents = _context.UpdateStatus.OrderBy(r => r.Name).AsEnumerable();
            var uniquelist = new List<string>();
            //foreach (UpdateStatus item in allEvents)
            //{
            //    if (uniquelist.Contains(item.Name) == false && item.Name != "test")
            //    {
            //        uniquelist.Add(item.Name);
            //    }
            //}

            uniquelist.Add("tafelvoetbal");

            return uniquelist.AsEnumerable();
        }



        IEnumerable<ChannelEvent> IUpdate.GetTimeSpanByName(string name, DateTime startDateTime, DateTime endDateTime)
        {
            throw new NotImplementedException();
        }


        public ChannelUser Add(ChannelUser updateStatusContent)
        {
            _context.ChannelUser.Add(updateStatusContent);
            _context.SaveChanges();
            return updateStatusContent;
        }

        public ChannelUser Update(ChannelUser updateStatusContent)
        {
            _context.Attach(updateStatusContent).State = EntityState.Modified;
            _context.SaveChanges();
            return updateStatusContent;
        }



        public ChannelEvent AddOrUpdate(InputChannelEvent inputChannelEventContent)
        {

            if (IsUserInDatabase(inputChannelEventContent.Name))
            {

                var userid = GetUserIdByUrlSafeName(inputChannelEventContent.Name);
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

        //public IEnumerable<ChannelUser> GetUserListByName(string name)
        //{
        //    return _context.ChannelUser.Where(b => b.Name == name);
        //}

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

        public string GetUserIdByUrlSafeName(string nameUrlSafe)
        {
            var userIdObject = _context.ChannelUser.LastOrDefault(p => p.NameUrlSafe == nameUrlSafe);
            return userIdObject?.NameId;
            // return null if not there
        }

        public ChannelUser AddUser()
        {

            var newChannelUser = new ChannelUser
            {
                Name = "Human Name",
                IsVisible = true,
                IsAccessible = true,
                NameUrlSafe = "urlname"
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

        public GetStatus IsFree(string nameUrlSafe)
        {
            var latestEvent = _context.ChannelEvent
                .Where(b => b.ChannelUser.NameUrlSafe == nameUrlSafe)
                .Include(b => b.ChannelUser)
                .LastOrDefault();

            if (latestEvent == null)
            {
                throw new FileNotFoundException();
            }

            var difference = DateTime.UtcNow - latestEvent.DateTime;

            var isFreeStatus = new GetStatus
            {
                Name = latestEvent.ChannelUser.Name,
                DateTime = latestEvent.DateTime,
                Difference = difference
            };

            if (difference.Minutes > 2)
            {
                isFreeStatus.IsFree = true;
            }

            return isFreeStatus;
        }



        public IEnumerable<ChannelEventsModel> GetTimeSpanByName(string nameid, DateTime startDateTime, DateTime endDateTime)
        {
            var test = new List<ChannelEventsModel>();

            return test;
        }

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




        //public ChannelEvent GetLatestByName(string nameid)
        //{
        //    var getLatestDateTimeByName = _context.ChannelEvent.LastOrDefault(b => b.NameId == nameid);
        //    if (getLatestDateTimeByName == null) return new ChannelEvent();

        //    var lastObject = _context.ChannelEvent
        //        .LastOrDefault(b => b.NameId == nameid);

        //    if (lastObject == null) return new ChannelEvent();

        //    return lastObject;
        //}


        //public UpdateStatus GetLatestByName(string name)
        //{
        //    return _context.UpdateStatus
        //            .LastOrDefault(b => b.Name == name);
        //}


        //public IEnumerable<UpdateStatus> GetRecentByName(string name)
        //{
        //    return _context.UpdateStatus
        //        .Where(p => p.DateTime > DateTime.UtcNow.Subtract(new TimeSpan(0, 8, 0, 0)))
        //        .Where(b => b.Name == name);
        //}

        //public IEnumerable<UpdateStatus> GetTimeSpanByName(string name, DateTime startDateTime, DateTime endDateTime)
        //{
        //    var result = _context.UpdateStatus
        //        .Where(p => p.DateTime >= startDateTime && p.DateTime <= endDateTime)
        //        .Where(b => b.Name == name);
        //    return result;
        //}


    }
}