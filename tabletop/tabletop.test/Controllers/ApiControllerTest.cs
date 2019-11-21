using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tabletop.Controllers;
using tabletop.Data;
using tabletop.Dtos;
using tabletop.Models;
using tabletop.Services;
using tabletop.ViewModels;

namespace tabletop.tests.Controllers
{
	[TestClass]
	public class ApiControllerTest
	{
		private readonly IMemoryCache _memoryCache;
		private readonly AppDbContext _context;
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
		
			var existAccount = _sqlStatus.GetChannelUserIdByUrlSafeName("testaccount", true);
			if(existAccount != null) return;
			
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
		
		[TestMethod]
		public void EventsRecent_NotFound_Test()
		{
			var eventsRecentJson = new ApiController(_sqlStatus, null).EventsRecent(new DateDto{Name = "notfound", Date = "0"}) as BadRequestObjectResult;
			Assert.AreEqual(400,eventsRecentJson.StatusCode);
		}
		
		[TestMethod]
		public void EventsDayView_csv_Test()
		{
			var eventsRecentJson = new ApiController(_sqlStatus, null)
				.EventsDayView(new DateDto{Name = "testaccount", Date = "0"},"csv") as ContentResult;
			var eventsRecent = eventsRecentJson.Content;
			
			Assert.IsTrue(eventsRecent.Contains("DateTime;Weight;Label"));
			Assert.IsTrue(eventsRecent.Contains(";1;"));
		}
		
		[TestMethod]
		public void EventsDayView_json_Test()
		{
			var eventsRecentJson = new ApiController(_sqlStatus, null)
				.EventsDayView(new DateDto{Name = "testaccount", Date = "0"},"json") as JsonResult;
			var eventsRecent = eventsRecentJson.Value as EventsOfficeHoursModel;
			var firstWeight = eventsRecent.AmountOfMotions.LastOrDefault(p => p.Weight == 1);
			
			Assert.AreEqual(1,firstWeight.Weight);
		}
		
		[TestMethod]
		public void EventsOfficeHours_Test()
		{
			var eventsEventsOfficeJson = new ApiController(_sqlStatus, null)
				.EventsOfficeHours(new DateDto{Name = "testaccount", Date = "0"}) as JsonResult;
			var eventsEventsOffice = eventsEventsOfficeJson.Value as EventsOfficeHoursModel;
			
			Assert.AreEqual(109,eventsEventsOffice.AmountOfMotions.Count);
		}
		
		[TestMethod]
		public void Export_Test()
		{
			_context.ChannelUser.FirstOrDefault(p => p.NameUrlSafe == "testaccount").Bearer =
				"fake_token_here";
			
			var apiController = new ApiController(_sqlStatus, null)
			{
				ControllerContext =
					new ControllerContext {HttpContext = new DefaultHttpContext()}
			};
			apiController.ControllerContext.HttpContext.Request.Headers["Authorization"] = "fake_token_here"; 
			
			var eventsExportJson = apiController
				.Export("testaccount","json") as JsonResult;
			var eventsExport = eventsExportJson.Value as EventsOfficeHoursModel;
			
			var firstWeight = eventsExport.AmountOfMotions.LastOrDefault(p => p.Weight == 1);
			Assert.AreEqual(1,firstWeight.Weight);
		}

		[TestMethod]
		public void IsFree_Test()
		{
			
			var eventsRecentJson = new ApiController(_sqlStatus, null)
				.IsFree("testaccount") as JsonResult;
			var eventsRecent = eventsRecentJson.Value as GetStatus;
			Assert.IsTrue(eventsRecent.Difference <= new TimeSpan(0,1,0));
		}
	}
}
