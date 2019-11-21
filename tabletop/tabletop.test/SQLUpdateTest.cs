using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
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
            builder.UseInMemoryDatabase(nameof(SqlUpdateTest));
            var options = builder.Options;
	        
	        var provider = new ServiceCollection()
		        .AddMemoryCache()
		        .BuildServiceProvider();
	        _memoryCache = provider.GetService<IMemoryCache>();

            _context = new AppDbContext(options);
            _sqlStatus = new SqlUpdateStatus(_context,_memoryCache);
        }

        private readonly AppDbContext _context;
        private readonly SqlUpdateStatus _sqlStatus;
	    private readonly IMemoryCache _memoryCache;

	    [TestMethod]
        public void AddTestAccountUserTest() { 
            _sqlStatus.AddUser("Test Account");
            var userIdChannelUser = _sqlStatus.GetChannelUserIdByUrlSafeName("testaccount", true);
            Assert.AreEqual(userIdChannelUser.Name, "Test Account");
        }

        [TestMethod]
        public void AddTestAccountUserNullTest()
        {
            var addUserObject = _sqlStatus.AddUser(null);
            Assert.AreEqual(addUserObject, null);
        }

        public string AddTestAccountUserAndGetId()
        {
            var userIdChannelUser = _sqlStatus.GetChannelUserIdByUrlSafeName("testaccount", true);
            if ( !string.IsNullOrEmpty(userIdChannelUser.Name) ) return userIdChannelUser.NameId;
            _sqlStatus.AddUser("Test Account");
            return _sqlStatus.GetChannelUserIdByUrlSafeName("testaccount", true).NameId;;
        }

        [TestMethod]
        public void AddOrUpdateTest()
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
        public void IsFreeFalseRecentCallTest()
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
        public void IsFreeTrueEventLongTimeAgoTest()
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
