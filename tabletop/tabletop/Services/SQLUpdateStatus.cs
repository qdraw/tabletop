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
            //_context.UpdateStatus.Add(UpdateStatusContent);
            //_context.SaveChanges();
            return UpdateStatusContent;
        }

        //public Restaurant Get(int id)
        //{
        //    return _context.Restaurant.FirstOrDefault(r => r.Id == id);
        //}

        //public IEnumerable<Restaurant> getAll()
        //{
        //    return _context.Restaurant.OrderBy(r => r.Name);
        //}
    }
}
