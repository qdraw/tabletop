using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest.Azure;
using tabletop.Dtos;
using tabletop.Interfaces;
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

        public ApiController(IUpdate updateStatusContent, IConfiguration iconfiguration)
        {
            _updateStatusContent = updateStatusContent;
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
        public IActionResult EventsDayView(DateDto dto)
        {
            var dateTime = dto.GetDateTime();
            if (string.IsNullOrEmpty(dto.Name) && dateTime.Year > 2015) return BadRequest("name or date wrong");

            var result =
                _updateStatusContent.EventsDayView(dateTime, dto.Name);
            if (result == null) return BadRequest("name error");
            return Json(result);
        }


        //[HttpGet]
        //[Produces("application/json")]
        //public IActionResult EventsOfficeHours(DateDto dto)
        //{

        //    var dateTime = dto.GetDateTime();
        //    if (string.IsNullOrEmpty(dto.Name) && dateTime.Year > 2015 ) return BadRequest("name or date wrong");

        //    var startDateTime = dateTime.ToUniversalTime().AddHours(9);
        //    var endDateTime = dateTime.ToUniversalTime().AddHours(18);

        //    var result =
        //        _updateStatusContent.Events(startDateTime, endDateTime, dto.Name);

        //    if (result == null) return BadRequest("name error");

        //    return Json(result);
        //}
        
        public IActionResult Index()
        {
            return View();
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
        public IActionResult Update(InputChannelEvent model)
        {

            if (!ModelState.IsValid) return BadRequest("Model is incomplete");

            var bearerValid = IsBearerValid(Request,model.Name);
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
