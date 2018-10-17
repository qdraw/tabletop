using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tabletop.Data;
using tabletop.Services;
using tabletop.Models;



namespace tabletop.tests
{
    [TestClass]
    public class SqlUpdateTest
    {

        public SqlUpdateTest()
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase();
            var _options = builder.Options;

            _context = new AppDbContext(_options);
            _sqlStatus = new SqlUpdateStatus(_context,null);
        }

        private readonly AppDbContext _context;
        private readonly SqlUpdateStatus _sqlStatus;

        [TestMethod]
        public void AddTestAccountUser() { 
            _sqlStatus.AddUser("Test Account");
            var userIdChannelUser = _sqlStatus.GetChannelUserIdByUrlSafeName("testaccount", true);
            Assert.AreEqual(userIdChannelUser.Name, "Test Account");
        }

        [TestMethod]
        public void AddTestAccountUserNull()
        {
            var addUserObject = _sqlStatus.AddUser(null);
            Assert.AreEqual(addUserObject, null);
        }

        public string AddTestAccountUserAndGetId()
        {
            _sqlStatus.AddUser("Test Account");
            var userIdChannelUser = _sqlStatus.GetChannelUserIdByUrlSafeName("testaccount", true);
            return  userIdChannelUser.NameId;
        }

        [TestMethod]
        public void AddOrUpdate()
        {
            AddTestAccountUserAndGetId();

            var newUpdateStatus = new InputChannelEvent()
            {
                Status = 1,
                Name = "testaccount"
            };

            _sqlStatus.AddOrUpdate(newUpdateStatus);
            var countItemsInDatabase = _context.ChannelEvent.OrderBy(r => r.Id).Count();
            Assert.AreEqual(countItemsInDatabase, 1);

            _sqlStatus.AddOrUpdate(newUpdateStatus);
            countItemsInDatabase = _context.ChannelEvent.OrderBy(r => r.Id).Count();
            Assert.AreEqual(countItemsInDatabase, 1);
        }

        [TestMethod]
        public void IsFreeFalseRecentCall()
        {
            var userid = AddTestAccountUserAndGetId();

            var newUpdateStatus = new InputChannelEvent
            {
                Status = 1,
                Name = "testaccount"
            };
            _sqlStatus.AddOrUpdate(newUpdateStatus);

            var isFreeNow = _sqlStatus.IsFree(userid);
            Assert.AreEqual(isFreeNow.IsFree, false);
        }

        [TestMethod]
        public void IsFreeTrueEventLongTimeAgo()
        {
            var userid = AddTestAccountUserAndGetId();

            var newUpdateStatus = new InputChannelEvent
            {
                Status = 1,
                Name = "testaccount"
            };
            var newAddedObject = _sqlStatus.AddOrUpdate(newUpdateStatus);

            newAddedObject.DateTime = new DateTime(2015,01,01,00,00,00);
            _sqlStatus.Update(newAddedObject);

            var isFreeNow = _sqlStatus.IsFree(userid);
            Assert.AreEqual(isFreeNow.IsFree, true);
        }
    }

}
