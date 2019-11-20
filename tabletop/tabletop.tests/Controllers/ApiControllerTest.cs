using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tabletop.Controllers;
using tabletop.Data;
using tabletop.Dtos;
using tabletop.Interfaces;
using tabletop.Models;
using tabletop.Services;
using tabletop.ViewModels;

namespace tabletop.tests.Controllers
{
	[TestClass]
	public class ApiControllerTest
	{
		private IMemoryCache _memoryCache;
		private AppDbContext _context;
		private SqlUpdateStatus _sqlStatus;

		public ApiControllerTest()
		{
			var builder = new DbContextOptionsBuilder<AppDbContext>();
			builder.UseInMemoryDatabase(nameof(ApiControllerTest));
			var options = builder.Options;
	        
			var provider = new ServiceCollection()
				.AddMemoryCache()
				.BuildServiceProvider();
			_memoryCache = provider.GetService<IMemoryCache>();

			_context = new AppDbContext(options);
			_sqlStatus = new SqlUpdateStatus(_context,_memoryCache);
			
			// Add example data
			_sqlStatus.AddUser("Test Account");
			
			var newUpdateStatus = new InputChannelEvent
			{
				Status = 1,
				Name = "testaccount"
			};
			_sqlStatus.AddOrUpdate(newUpdateStatus);
		}
		

		[TestMethod]
		public void IndexTest()
		{
			var index = new ApiController(_sqlStatus, null).Index() as NotFoundResult;
			Assert.AreEqual(404, index.StatusCode);
		}
		
		[TestMethod]
		public void EnvTest()
		{
			var index = new ApiController(_sqlStatus, null).Env() as OkResult;
			Assert.AreEqual(200, index.StatusCode);
		}
		
		[TestMethod]
		public void EventsRecentTest()
		{
			var eventsRecentJson = new ApiController(_sqlStatus, null).EventsRecent(new DateDto{Name = "testaccount", Date = "0"}) as JsonResult;
			var eventsRecent = eventsRecentJson.Value as EventsOfficeHoursModel;
			var firstWeight = eventsRecent.AmountOfMotions.LastOrDefault(p => p.Weight == 1);
			
			Assert.AreEqual(1,firstWeight.Weight);
		}
		
	}
}
