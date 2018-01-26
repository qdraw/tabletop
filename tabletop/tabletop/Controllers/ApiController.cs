using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Rest.Azure;
using tabletop.Dtos;
using tabletop.Interfaces;
using tabletop.Models;
using tabletop.ViewModels;

///*
//There is no ApiController class anymore since MVC and WebAPI have been merged in ASP.NET Core.
//However, the Controller class of MVC brings in a bunch of features you probably won't need
//when developing just a Web API, such as a views and model binding.
//For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//https://stackoverflow.com/questions/38667445/is-apicontroller-deprecated-in-net-core/38672681
//*/

namespace tabletop.Controllers
{
    public class ApiController : Controller
    {
        private readonly IUpdate _updateStatusContent;

        private readonly IConfiguration _iconfiguration;

        public ApiController(IUpdate updateStatusContent, IConfiguration iconfiguration)
        {
            _updateStatusContent = updateStatusContent;
            _iconfiguration = iconfiguration;

        }

        // todo: duplicate code and loop
        [HttpGet]
        [Produces("application/json")]
        public IActionResult EventsRecent(DateDto dto)
        {
            //return BadRequest("tabletop fails");


            var startDateTime = dto.RoundDown(DateTime.UtcNow.Subtract(new TimeSpan(0, 8, 0, 0)), new TimeSpan(0, 0, 5, 0));
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


        // todo: duplicate code and loop
        [HttpGet]
        [Produces("application/json")]
        public IActionResult EventsOfficeHours(DateDto dto)
        {

            var dateTime = dto.GetDateTime();
            const int interval = 60 * 5; // 5 minutes
            if (!string.IsNullOrEmpty(dto.Name) && dateTime.Year > 2015)
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
                    AmountOfMotions = new List<WeightViewModel>()
                };

                var i = dto.GetUnixTime(startDateTime);
                while (i <= dto.GetUnixTime(endDateTime))
                {

                    var startIntervalDateTime = dto.UnixTimeToDateTime(i);
                    var endIntervalDateTime = dto.UnixTimeToDateTime(i + interval);

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




        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Produces("application/json")]
        public IActionResult IsFree(string name)
        {
            var nameUrlSafe = name;

            if (!string.IsNullOrEmpty(nameUrlSafe))
            {
                var newStatusContent = _updateStatusContent.IsFree(nameUrlSafe);
                return Json(newStatusContent);
            }
            else
            {
                return BadRequest("name is invalid");
            }

        }



        [HttpPost]
        [Produces("application/json")]
        public IActionResult Update(InputChannelEvent model)
        {
            var bearerValid = IsBearerValid(Request);

            if (!ModelState.IsValid) return BadRequest("Model is incomplete");
            if (!bearerValid) return BadRequest("Authorisation Error");

            try
            {
                var newStatusContent = _updateStatusContent.AddOrUpdate(model);
                return Ok(newStatusContent.Weight);
            }
            catch (CloudException)
            {
                return new StatusCodeResult(500);
            }
            catch (NotImplementedException)
            {
                return BadRequest("Name does not exist");
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
