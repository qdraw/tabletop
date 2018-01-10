using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tabletop.Data;
using tabletop.Interfaces;
using tabletop.Models;

namespace tabletop.Services
{
    public class SQLUpdateStatus : IUpdateStatus
    {
        private appDbContext _context;

        public SQLUpdateStatus(appDbContext context)
        {
            _context = context;
        }



        public UpdateStatus Add(UpdateStatus UpdateStatusContent)
        {
            UpdateStatusContent.Weight = 0;
            _context.UpdateStatus.Add(UpdateStatusContent);
            _context.SaveChanges();
            return UpdateStatusContent;

        }

        //if (lastMinuteCount >= 1)
        //{
        //    //foreach (var r in lastMinuteRequests)
        //    //{
        //    //    r.Weight = lastMinuteCount;
        //    //}

        //    UpdateStatusContent.Weight = UpdateStatusContent.Weight++;
        //    UpdateStatusContent.Id = lastMinuteRequests.FirstOrDefault().Id;
        //    UpdateStatusContent.Name = lastMinuteRequests.FirstOrDefault().Name;
        //    UpdateStatusContent.Status = 2;

        //    //_context.Attach(UpdateStatusContent).State = EntityState.Modified;
        //    //_context.SaveChanges();



        //    //var newUpdateStatus = new UpdateStatus();
        //    //newUpdateStatus.Weight++;
        //    //newUpdateStatus.Id = lastMinuteRequests.FirstOrDefault().Id;
        //    //newUpdateStatus.Name = lastMinuteRequests.FirstOrDefault().Name;
        //    //newUpdateStatus.Status = 1;
        //    //newUpdateStatus.DateTime = lastMinuteRequests.FirstOrDefault().DateTime;
        //    //var db = new appDbContext();
        //    //_context.Update(newUpdateStatus);
        //    //_context.SaveChanges();

        //    //_context.UpdateStatus.Update(newUpdateStatus);

        //    return UpdateStatusContent;

        //}

        //else
        //{

        //    UpdateStatusContent.Weight = lastMinuteCount;


        //}


        public IEnumerable<UpdateStatus> getLastMinute(string name)
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

        public IEnumerable<UpdateStatus> getAll()
        {
            return _context.UpdateStatus.OrderBy(r => r.Id);
        }

        public IEnumerable<UpdateStatus> getAllByName(string name)
        {
            return _context.UpdateStatus.Where(b => b.Name == name);
        }

        public UpdateStatus GetLatestByName(string name)
        {
            return _context.UpdateStatus.Where(b => b.Name == name)
                    .LastOrDefault();
        }

        public IEnumerable<UpdateStatus> getRecentByName(string name)
        {
            return _context.UpdateStatus
                .Where(p => p.DateTime > DateTime.Now.Subtract(new TimeSpan(0, 8, 0, 0)))
                .Where(b => b.Name == name);
        }

        public UpdateStatus Update(UpdateStatus UpdateStatusContent)
        {
            _context.Attach(UpdateStatusContent).State = EntityState.Modified;
            _context.SaveChanges();
            return UpdateStatusContent;
        }

        //public IEnumerable<RecentStatusList> GetRecentStatusList()
        //{
        //    return _context.UpdateStatus..OrderBy(r => r.Name);
        //}


    }
}
