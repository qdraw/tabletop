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
            _context.UpdateStatus.Add(UpdateStatusContent);
            _context.SaveChanges();
            return UpdateStatusContent;
        }

        public UpdateStatus Get(int id)
        {
            return _context.UpdateStatus.FirstOrDefault(r => r.Id == id);
        }

        public UpdateStatus GetLatestByName(string name)
        {
            return _context.UpdateStatus.Where(b => b.Name == name)
                    .LastOrDefault();
        }

        //public IEnumerable<Restaurant> getAll()
        //{
        //    return _context.Restaurant.OrderBy(r => r.Name);
        //}
    }
}
