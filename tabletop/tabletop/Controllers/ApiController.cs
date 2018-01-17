using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using tabletop.Interfaces;
using tabletop.Models;


namespace tabletop.Controllers
{
    public class ApiController : Controller
    {
        private readonly IUpdateStatus _updateStatusContent;

        readonly IConfiguration _iconfiguration;

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


        public IActionResult Details(int id)
        {
            var model = _updateStatusContent.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetLatestByName(string name)
        {
            var model = _updateStatusContent.GetLatestByName(name);
            if (model == null)
            {
                return NotFound();
            }
            return Json(model.DateTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss+00:00"));

            // return Content(model.DateTime.ToString());
            // return View(model);
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetUnixTime()
        {
            var unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return Json(unixTimestamp);

        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetUniqueNames()
        {
            return Json(_updateStatusContent.GetUniqueNames());
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetAll(string name)
        {

            if (string.IsNullOrEmpty(name))
            {
                var model = new RecentStatusClass();
                model.RecentStatus = _updateStatusContent.GetAll();
                var listOf = model.RecentStatus.ToList();
                return Json(listOf);
            }

            else {
                var model = new RecentStatusClass();
                model.RecentStatus = _updateStatusContent.GetAllByName(name);
                var listOf = model.RecentStatus.ToList();
                return Json(listOf);
            }

        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetRecentByName(string name)
        {
            var model = new RecentStatusClass {RecentStatus = _updateStatusContent.GetRecentByName(name)};
            var listOf = model.RecentStatus.ToList();

            return Json(listOf);

        }


        [HttpGet]
        [Produces("application/json")]
        public IActionResult ListOfWorkDayByName(string name, string date)
        {
            if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(name))
            {
                var dateTime = new DateTime();
                DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);

                if (dateTime.Year <= 2000) return BadRequest("date fails");
                var model = new RecentStatusClass
                {
                    RecentStatus = _updateStatusContent.ListOfWorkDayByName(name, dateTime)
                };
                var listOf = model.RecentStatus.ToList();

                return Json(listOf);

            }
            return BadRequest("date or name fails");
        }


        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetLastMinute(string name)
        {
            var model = new RecentStatusClass {RecentStatus = _updateStatusContent.GetLastMinute(name)};
            var listOf = model.RecentStatus.ToList();
            return Json(listOf);

        }

        
        public IActionResult Index()
        {
            //var model = new HomeIndexViewModel();
            //model.Restaurants = _restaurantData.getAll();
            //model.CurrentMessage = _greeter.GetTheMessageOfTheDay();
            //return View(model);
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

            var newStatusContent = _updateStatusContent.AddOrUpdate(model);
            if (newStatusContent.Status == -400)
            {
                return BadRequest("Database Error: -400");

            }
            return Json(newStatusContent);

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
