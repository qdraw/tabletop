using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using tabletop.Data;
using tabletop.Interfaces;
using tabletop.Models;

namespace tabletop.Services
{
    public class SqlUpdateStatus : IUpdateStatus
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


        public UpdateStatus Add(UpdateStatus updateStatusContent)
        {
            _context.UpdateStatus.Add(updateStatusContent);
            _context.SaveChanges();
            return updateStatusContent;

        }

        public UpdateStatus Update(UpdateStatus updateStatusContent)
        {
            _context.Attach(updateStatusContent).State = EntityState.Modified;
            _context.SaveChanges();
            return updateStatusContent;
        }

        public UpdateStatus AddOrUpdate(UpdateStatus updateStatusContent)
        {
            return new UpdateStatus();
        }

        //public UpdateStatus AddOrUpdate(UpdateStatus updateStatusContent)
        //{
        //    var getLastMinuteContent = GetLastMinute(updateStatusContent.Name);
        //    var lastMinuteContent = getLastMinuteContent.ToList();
        //    var firstLastMinuteContent = lastMinuteContent.FirstOrDefault();

        //    if (!lastMinuteContent.Any())
        //    {
        //        var newStatusContent = new UpdateStatus
        //        {
        //            Name = updateStatusContent.Name,
        //            Status = updateStatusContent.Status,
        //            DateTime = DateTime.UtcNow,
        //            Weight = 1
        //        };

        //        _context.UpdateStatus.Add(newStatusContent);
        //        _context.SaveChanges();
        //        return newStatusContent;
        //    }

        //    if (firstLastMinuteContent == null)
        //        throw new DbUpdateException("", new Exception("request from database went wrong"));

        //    firstLastMinuteContent.Weight++;
        //    firstLastMinuteContent.DateTime = DateTime.UtcNow;
        //    _context.Attach(firstLastMinuteContent).State = EntityState.Modified;
        //    _context.SaveChanges();
        //    return firstLastMinuteContent;
        //}



        //public IEnumerable<UpdateStatus> GetLastMinute(string name)
        //{
        //    // round minute to 18:48 for example
        //    var now = DateTime.UtcNow;
        //    var nowTicks = now.Ticks;

        //    var lastMinute = new DateTime(nowTicks - (nowTicks % (1000 * 1000 * 10 * 60))); // 60
        //    //var lastMinute = DateTime.UtcNow.Subtract(new TimeSpan(0, 0, 30, 0));

        //    var lastMinuteRequests = _context.UpdateStatus
        //        .Where(p => p.DateTime > lastMinute)
        //        .Where(b => b.Name == name);

        //    //var lastMinuteCount = lastMinuteRequests.ToArray().Length;
        //    return lastMinuteRequests;
        //}

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

        //public GetStatus IsFree(string name)
        //{
        //    var isFreeStatus = new GetStatus {Name = name};

        //    var isFree = _context.UpdateStatus
        //        .Where(p => p.DateTime > DateTime.UtcNow.Subtract(new TimeSpan(0, 0, 2, 0)))
        //        .Where(b => b.Name == name).ToArray();

        //    isFreeStatus.IsFree = !isFree.Any();

        //    var lastByName = GetLatestDateTimeByName(name);
        //    isFreeStatus.DateTime = lastByName;

        //    return isFreeStatus;
        //}

        public GetStatus IsFree(string name)
        {
            var isFreeStatus = new GetStatus {Name = name};
            isFreeStatus.IsFree = false;
            return isFreeStatus;
        }


        //public UpdateStatus GetLatestByName(string name)
        //{
        //    return _context.UpdateStatus
        //            .LastOrDefault(b => b.Name == name);
        //}

        //public DateTime GetLatestDateTimeByName(string name)
        //{
        //    var getLatestDateTimeByName = _context.UpdateStatus.LastOrDefault(b => b.Name == name);
        //    if (getLatestDateTimeByName == null) return new DateTime();

        //    var lastObject = _context.UpdateStatus
        //        .LastOrDefault(b => b.Name == name);
        //    return lastObject?.DateTime ?? new DateTime();
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

        public IEnumerable<UpdateStatus> GetTimeSpanByName(string name, DateTime startDateTime, DateTime endDateTime)
        {
            var test = new List<UpdateStatus>();

            return test;
        }
    }
}