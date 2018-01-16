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
            var allEvents = _context.UpdateStatus.OrderBy(r => r.Name).AsEnumerable();
            var uniquelist = new List<string>();
            foreach (UpdateStatus item in allEvents)
            {
                if (uniquelist.Contains(item.Name) == false && item.Name != "test")
                {
                    uniquelist.Add(item.Name);
                }
            }
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
            var getLastMinuteContent = GetLastMinute(updateStatusContent.Name);

            if (getLastMinuteContent.ToArray().Length == 0)
            {
                var newStatusContent = new UpdateStatus();
                newStatusContent.Name = updateStatusContent.Name;
                newStatusContent.Status = updateStatusContent.Status;
                newStatusContent.DateTime = DateTime.UtcNow;
                newStatusContent.Weight = 1;
                _context.UpdateStatus.Add(newStatusContent);
                _context.SaveChanges();
                return newStatusContent;
            }
            else
            {
                getLastMinuteContent.FirstOrDefault().Weight++;
                getLastMinuteContent.FirstOrDefault().DateTime = DateTime.UtcNow;
                _context.Attach(getLastMinuteContent.FirstOrDefault()).State = EntityState.Modified;
                _context.SaveChanges();
                return getLastMinuteContent.FirstOrDefault();
            }
        }



        public IEnumerable<UpdateStatus> GetLastMinute(string name)
        {
            // round minute to 18:48 for example
            var now = DateTime.UtcNow;
            var nowTicks = now.Ticks;

            var lastMinute = new DateTime(nowTicks - (nowTicks % (1000 * 1000 * 10 * 60))); // 60
            //var lastMinute = DateTime.UtcNow.Subtract(new TimeSpan(0, 0, 30, 0));

            // UpdateStatusContent.Name.ToString()
            var lastMinuteRequests = _context.UpdateStatus
                .Where(p => p.DateTime > lastMinute)
                .Where(b => b.Name == name);

            //var lastMinuteCount = lastMinuteRequests.ToArray().Length;
            return lastMinuteRequests;
        }

        public UpdateStatus Get(int id)
        {
            return _context.UpdateStatus.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<UpdateStatus> GetAll()
        {
            return _context.UpdateStatus.OrderBy(r => r.Id);
        }

        public IEnumerable<UpdateStatus> GetAllByName(string name)
        {
            return _context.UpdateStatus.Where(b => b.Name == name);
        }

        public GetStatus IsFree(string name)
        {
            var IsFreeStatus = new GetStatus();
            IsFreeStatus.Name = name;

            var isFree = _context.UpdateStatus
                .Where(p => p.DateTime > DateTime.UtcNow.Subtract(new TimeSpan(0, 0, 2, 0)))
                .Where(b => b.Name == name).ToArray();
           
            IsFreeStatus.IsFree = !isFree.Any();

            var lastByName = GetLatestDateTimeByName(name);
            IsFreeStatus.DateTime = lastByName;

            return IsFreeStatus;
        }


        public UpdateStatus GetLatestByName(string name)
        {
            return _context.UpdateStatus
                    .LastOrDefault(b => b.Name == name);
        }

        public DateTime GetLatestDateTimeByName(string name)
        {

            var getLatestDateTimeByName = _context.UpdateStatus.LastOrDefault(b => b.Name == name);
            if (getLatestDateTimeByName != null)
            {
                return _context.UpdateStatus
                    .LastOrDefault(b => b.Name == name).DateTime;
            }
            else
            {
                return new DateTime();

            }
        }
        public IEnumerable<UpdateStatus> GetRecentByName(string name)
        {
            return _context.UpdateStatus
                .Where(p => p.DateTime > DateTime.UtcNow.Subtract(new TimeSpan(0, 8, 0, 0)))
                .Where(b => b.Name == name);
        }

    }
}
