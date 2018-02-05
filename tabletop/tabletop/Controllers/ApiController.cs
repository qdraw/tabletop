using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest.Azure;
using tabletop.Dtos;
using tabletop.Interfaces;
using tabletop.MessageHandler;
using tabletop.Models;

//There is no ApiController class anymore since MVC and WebAPI have been merged in ASP.NET Core.
//However, the Controller class of MVC brings in a bunch of features you probably won't need
//when developing just a Web API, such as a views and model binding.
//For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//https://stackoverflow.com/questions/38667445/is-apicontroller-deprecated-in-net-core/38672681


namespace tabletop.Controllers
{
    public class ApiController : Controller
    {
        private readonly IUpdate _updateStatusContent;

        private NotificationsMessageHandler NotificationsMessageHandler { get; }

        public ApiController(IUpdate updateStatusContent, NotificationsMessageHandler notificationsMessageHandler)
        {
            _updateStatusContent = updateStatusContent;
            NotificationsMessageHandler = notificationsMessageHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult EventsRecent(DateDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name)) return BadRequest("name wrong");
            var result = _updateStatusContent.EventsRecent(dto.Name);
            if (result == null) return BadRequest("name error");
            return Json(result);
        }


        [HttpGet]
        [Produces("application/json")]
        public IActionResult EventsDayView(DateDto dto, string ext)
        {
            var dateTime = dto.GetDateTime();
            if (string.IsNullOrEmpty(dto.Name) && dateTime.Year > 2015) return BadRequest("name or date wrong");

            var result =
                _updateStatusContent.EventsDayView(dateTime, dto.Name);
            if (result == null) return BadRequest("name error");

            switch (ext)
            {
                case "csv":
                    var resultCsv = "DateTime;Weight;Label\n";
                    foreach (var item in result.AmountOfMotions)
                    {
                        resultCsv += $"{item.StartDateTime};{item.Weight};{item.Label}\n";
                    }
                    return Content(resultCsv);

                case "json":
                    return Json(result);
                default:
                    return Json(result);
            }
        }


        [HttpGet]
        [Produces("application/json")]
        public IActionResult EventsOfficeHours(DateDto dto)
        {
            var dateTime = dto.GetDateTime();
            if (string.IsNullOrEmpty(dto.Name) && dateTime.Year > 2015) return BadRequest("name or date wrong");

            var startDateTime = dateTime.ToUniversalTime().AddHours(9);
            var endDateTime = dateTime.ToUniversalTime().AddHours(18);

            var getDataChannelEvents = _updateStatusContent.GetTimeSpanByName(dto.Name, startDateTime, endDateTime).ToList();
            var result = _updateStatusContent.ParseEvents(getDataChannelEvents, startDateTime, endDateTime);

            if (result == null) return BadRequest("name error");

            return Json(result);
        }


        [HttpGet]
        public IActionResult Export(string name, string ext)
        {
            var getDataChannelEvents = _updateStatusContent.GetTimeSpanByName(name, new DateTime(2018, 01, 01), DateTime.Now).ToList();
            var result = _updateStatusContent.ParseEvents(getDataChannelEvents, new DateTime(2018, 01, 01), DateTime.Now);

            var bearerValid = IsBearerValid(Request, name);
            if (!bearerValid) return BadRequest("Authorisation Error");

            switch (ext)
            {
                case "csv":
                    var resultCsv = "DateTime;Weight;Label\n";
                    foreach (var item in result.AmountOfMotions)
                    {
                        resultCsv += $"{item.StartDateTime};{item.Weight};{item.Label}\n";
                    }
                    return Content(resultCsv);

                case "json":
                    return Json(result);
                default:
                    return Json(result);
            }
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult IsFree(string name)
        {
            var channelUserId = name;

            if (!string.IsNullOrEmpty(channelUserId))
            {
                var newStatusContent = _updateStatusContent.IsFree(channelUserId);
                return Json(newStatusContent);
            }
            else
            {
                return BadRequest("channelUserId is invalid");
            }

        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> Update(InputChannelEvent model)
        {

            if (!ModelState.IsValid) return BadRequest("Model is incomplete");

            var bearerValid = IsBearerValid(Request,model.Name);
            if (!bearerValid) return BadRequest("Authorisation Error");

            NotificationsMessageHandler.IsFree(model.Name);

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


        public bool IsBearerValid(Microsoft.AspNetCore.Http.HttpRequest request, string urlSafeName)
        {
            if ((Request.Headers["Authorization"].ToString() ?? "").Trim().Length > 0)
            {
                var bearer = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var channelUser = _updateStatusContent.GetChannelUserIdByUrlSafeName(urlSafeName,true);
                return channelUser.Bearer == bearer;
            }
            else
            {
                return false;
            }
        }

    }
}
