//using System;
//using System.Linq;
//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Storage.Internal;
//using Microsoft.Extensions.Configuration;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using tabletop;
//using tabletop.Data;
//using tabletop.Services;
//using tabletop.Models;



//namespace tabletop.tests
//{
//    [TestClass]
//    public class SqlUpdateTest
//    {
//        readonly DbContextOptions<AppDbContext> _options;

//        public SqlUpdateTest()
//        {
//            var builder = new DbContextOptionsBuilder<AppDbContext>();
//            builder.UseInMemoryDatabase();
//            _options = builder.Options;

//            _context = new AppDbContext(_options);
//            _sqlStatus = new SqlUpdateStatus(_context);
//        }

//        private AppDbContext _context;
//        private SqlUpdateStatus _sqlStatus;

//        [TestMethod]
//        public void AddOrUpdate()
//        {

//            var newUpdateStatus = new UpdateStatus
//            {
//                Status = 1,
//                Name = "test"
//            };

//            _sqlStatus.AddOrUpdate(newUpdateStatus);
//            var countItemsInDatabase = _context.UpdateStatus.OrderBy(r => r.Id).Count();
//            Assert.AreEqual(countItemsInDatabase, 1);


//            _sqlStatus.AddOrUpdate(newUpdateStatus);
//            countItemsInDatabase = _context.UpdateStatus.OrderBy(r => r.Id).Count();
//            Assert.AreEqual(countItemsInDatabase, 1);

//        }

//        [TestMethod]
//        public void IsFree()
//        {
//            var newUpdateStatus = new UpdateStatus
//            {
//                Status = 1,
//                Name = "test"
//            };

//            _sqlStatus.AddOrUpdate(newUpdateStatus);

//            var isFreeNow = _sqlStatus.IsFree("test");
//            Assert.AreEqual(isFreeNow.IsFree, false);
//        }
//    }

//}
