using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using tabletop.Dtos;
using tabletop.Interfaces;
using tabletop.Models;
using tabletop.ViewModels;

/*
There is no ApiController class anymore since MVC and WebAPI have been merged in ASP.NET Core.
However, the Controller class of MVC brings in a bunch of features you probably won't need
when developing just a Web API, such as a views and model binding.
For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
https://stackoverflow.com/questions/38667445/is-apicontroller-deprecated-in-net-core/38672681
*/

namespace tabletop.Controllers
{
    public class ApiController : Controller
    {
        private readonly IUpdateStatus _updateStatusContent;

        private readonly IConfiguration _iconfiguration;

        public ApiController(IUpdateStatus updateStatusContent, IConfiguration iconfiguration)
        {
            _updateStatusContent = updateStatusContent;
            _iconfiguration = iconfiguration;

        }
        //private IRestaurantData _restaurantData;
        //private IGreeter _greeter;

        //public HomeController(IRestaurantData restaurantData, IGreeter greeter)
        //{
        //    _restaurantData = restaurantData;
        //    _greeter = greeter;
        //}


        //public IActionResult Details(int id)
        //{
        //    var model = _updateStatusContent.Get(id);
        //    if (model == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(model);
        //}

        //[HttpGet]
        //[Produces("application/json")]
        //public IActionResult GetLatestByName(string name)
        //{
        //    var model = _updateStatusContent.GetLatestByName(name);
        //    if (model == null)
        //    {
        //        return NotFound();
        //    }
        //    return Json(model.DateTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss+00:00"));

        //    // return Content(model.DateTime.ToString());
        //    // return View(model);
        //}

        //[HttpGet]
        //[Produces("application/json")]
        //public IActionResult GetUnixTime()
        //{
        //    var unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        //    return Json(unixTimestamp);

        //}

        //[HttpGet]
        //[Produces("application/json")]
        //public IActionResult GetUniqueNames()
        //{
        //    return Json(_updateStatusContent.GetUniqueNames());
        //}

        //[HttpGet]
        //[Produces("application/json")]
        //public IActionResult GetAll(string name)
        //{

        //    if (string.IsNullOrEmpty(name))
        //    {
        //        var model = new RecentStatusClass();
        //        model.RecentStatus = _updateStatusContent.GetAll();
        //        var listOf = model.RecentStatus.ToList();
        //        return Json(listOf);
        //    }

        //    else {
        //        var model = new RecentStatusClass();
        //        model.RecentStatus = _updateStatusContent.GetAllByName(name);
        //        var listOf = model.RecentStatus.ToList();
        //        return Json(listOf);
        //    }

        //}

        [HttpGet]
        [Produces("application/json")]
        public IActionResult EventsRecent(DateDto dto)
        {

            //var statusContent = _updateStatusContent.GetRecentByName(dto.Name);



            var startDateTime = dto.RoundDown(DateTime.UtcNow.Subtract(new TimeSpan(0, 8, 0, 0)),new TimeSpan(0,0,5,0));
            var endDateTime = dto.RoundUp(DateTime.UtcNow, new TimeSpan(0, 0, 5, 0));


            var statusContent = _updateStatusContent.GetTimeSpanByName(
                dto.Name,
                startDateTime,
                endDateTime
            ).ToList().OrderBy(p => p.DateTime);

            var model = new EventsOfficeHoursModel
            {
                Day = startDateTime.DayOfWeek,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime,
                AmountOfMotions = new List<WeightViewModel>()
            };
            const int interval = 60 * 5; // 5 minutes

            var i = dto.GetUnixTime(model.StartDateTime);
            while (i <= dto.GetUnixTime(model.EndDateTime))
            {

                var startIntervalDateTime = dto.UnixTimeToDateTime(i);
                var endIntervalDateTime = dto.UnixTimeToDateTime(i + interval);

                var contentUpdateStatuses = statusContent.Where(p => p.DateTime > startIntervalDateTime && p.DateTime < endIntervalDateTime).ToList();

                var eventItem = new WeightViewModel();
                eventItem.StartDateTime = startIntervalDateTime;
                eventItem.EndDateTime = endIntervalDateTime;

                eventItem.Weight = 0;
                eventItem.Label = startIntervalDateTime.ToString("HH:mm");

                if (contentUpdateStatuses.Any())
                {
                    foreach (var item in contentUpdateStatuses)
                    {
                        eventItem.Weight += item.Weight;
                    }
                }

                model.AmountOfMotions.Add(eventItem);

                i += interval;
            }

            model.Length = model.AmountOfMotions.Count();
            
            return Json(model);
        }


        //[HttpGet]
        //[Produces("application/json")]
        //public IActionResult EventsRecent(DateDto dto)
        //{
        //    var model = new RecentStatusClass { RecentStatus = _updateStatusContent.GetRecentByName(dto.Name) };
        //    var dateTime = dto.GetDateTime();

        //    return Json(model.RecentStatus);
        //}



        [HttpGet]
        [Produces("application/json")]
        public IActionResult EventsOfficeHours(DateDto dto)
        {
            var dateTime = dto.GetDateTime();
            const int interval = 60 * 5; // 5 minutes
            if (!string.IsNullOrEmpty(dto.Name) && dateTime.Year > 2015 )
            {
                var startDateTime = dateTime.ToUniversalTime().AddHours(9);
                var endDateTime = dateTime.ToUniversalTime().AddHours(18);
                var statusContent = _updateStatusContent.GetTimeSpanByName(
                        dto.Name, 
                        startDateTime, 
                        endDateTime
                    ).ToList().OrderBy(p => p.DateTime);

                var model = new EventsOfficeHoursModel
                {
                    Day = dateTime.DayOfWeek,
                    StartDateTime = startDateTime,
                    EndDateTime = endDateTime,
                    AmountOfMotions =  new List<WeightViewModel>()
                };

                var i = dto.GetUnixTime(startDateTime);
                while (i <= dto.GetUnixTime(endDateTime))
                {

                    var startIntervalDateTime = dto.UnixTimeToDateTime(i);
                    var endIntervalDateTime = dto.UnixTimeToDateTime(i+interval);

                    var contentUpdateStatuses = statusContent.Where(p => p.DateTime > startIntervalDateTime && p.DateTime < endIntervalDateTime).ToList(); 
                    // Sum Weight Select-Statement >> Advies Joost!
                    //var contentUpdateStatusesExtended = statusContent
                    //    .Where(p => p.DateTime > startIntervalDateTime && p.DateTime < endIntervalDateTime)
                    //    .GroupBy(p => p.Weight)
                    //    .Select(p => new { WeightSum = p.Sum(c => c.Weight) }); // Sum Weight Select-Statement

                    var eventItem = new WeightViewModel();
                    eventItem.StartDateTime = startIntervalDateTime;
                    eventItem.EndDateTime = endIntervalDateTime;

                    //if (contentUpdateStatusesExtended != null && contentUpdateStatusesExtended.Any() && contentUpdateStatusesExtended.FirstOrDefault() != null)
                    //{
                    //    eventItem.Weight = contentUpdateStatusesExtended.FirstOrDefault().WeightSum;

                    //}

                    eventItem.Weight = 0;
                    eventItem.Label = startIntervalDateTime.ToString("HH:mm");

                    if (contentUpdateStatuses.Any())
                    {
                        foreach (var item in contentUpdateStatuses)
                        {
                            eventItem.Weight += item.Weight;
                        }
                    }

                    model.AmountOfMotions.Add(eventItem);

                    i += interval; 
                }

                model.Length = model.AmountOfMotions.Count();

                return Json(model);
            }
            return BadRequest("date or name fails");
        }


 

        //[HttpGet]
        //[Produces("application/json")]
        //public IActionResult GetLastMinute(string name)
        //{
        //    var model = new RecentStatusClass {RecentStatus = _updateStatusContent.GetLastMinute(name)};
        //    var listOf = model.RecentStatus.ToList();
        //    return Json(listOf);

        //}

        
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Produces("application/json")]
        public IActionResult IsFree(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var newStatusContent = _updateStatusContent.IsFree(name);
                return Json(newStatusContent);
            }
            else
            {
                return BadRequest("name is missing");
            }

        }



        [HttpPost]
        [Produces("application/json")]
        public IActionResult Update(UpdateStatus model)
        {
            var bearerValid = IsBearerValid(Request);

            if (!ModelState.IsValid) return BadRequest("Model is incomplete");
            if (!bearerValid) return BadRequest("Authorisation Error");

            try
            {
                var newStatusContent = _updateStatusContent.AddOrUpdate(model);
                return Json(newStatusContent);
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(500);
            }

            

        }


        public bool IsBearerValid(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            if ((Request.Headers["Authorization"].ToString() ?? "").Trim().Length > 0)
            {
                var bearer = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var bearerList = _iconfiguration.GetSection("bearer").Get<List<string>>();
                return bearerList.Exists(element => element == bearer);
            }
            else
            {
                return false;
            }
        }
    }
}
