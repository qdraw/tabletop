using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tabletop.Models;

namespace tabletop.Data
{
    public class appDbContext : DbContext
    {
        public appDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UpdateStatus> UpdateStatus { get; set; }
    }
}
