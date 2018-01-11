using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using tabletop.Interfaces;
using tabletop.Models;


namespace tabletop.Controllers
{
    public class ApiController : Controller
    {
        private IUpdateStatus _updateStatusContent;

        IConfiguration _iconfiguration;

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
        public IActionResult getUnixTime()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return Json(unixTimestamp);

        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult getAll(string name)
        {

            System.Diagnostics.Debug.WriteLine("name");
            System.Diagnostics.Debug.WriteLine(name);

            if (string.IsNullOrEmpty(name))
            {
                //System.Diagnostics.Debug.WriteLine(name.Length);
                var model = new RecentStatusClass();
                model.RecentStatus = _updateStatusContent.getAll();
                List<UpdateStatus> ListOf = model.RecentStatus.ToList();
                return Json(ListOf);
            }

            else {
                var model = new RecentStatusClass();
                model.RecentStatus = _updateStatusContent.getAllByName(name);
                List<UpdateStatus> ListOf = model.RecentStatus.ToList();
                return Json(ListOf);
            }

        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult getRecentByName(string name)
        {
            var model = new RecentStatusClass();
            model.RecentStatus = _updateStatusContent.getRecentByName(name);
            List<UpdateStatus> ListOf = model.RecentStatus.ToList();

            return Json(ListOf);

        }

        //[HttpGet]
        //[Produces("application/json")]
        //public IActionResult DistinctNames()
        //{
        //    var model = new RecentStatusClass();
        //    model.RecentStatus = _updateStatusContent.getRecentByName(name);
        //    List<UpdateStatus> ListOf = model.RecentStatus.ToList();

        //    return Json(ListOf);

        //}


        public IActionResult Index()
        {
            //var model = new HomeIndexViewModel();
            //model.Restaurants = _restaurantData.getAll();
            //model.CurrentMessage = _greeter.GetTheMessageOfTheDay();

            //return View(model);
            return View();
        }

        [HttpPost]
        public IActionResult Update(UpdateStatus model)
        {
            var BearerValid = IsBearerValid(Request);

            if (ModelState.IsValid && BearerValid)
            {

                var getLastMinuteContent = _updateStatusContent.getLastMinute(model.Name);

                var lenght1 = getLastMinuteContent.ToArray().Length;

                //getLastMinuteContent.Any();

                if (lenght1 == 0) {
                    var newStatusContent = new UpdateStatus();
                    newStatusContent.Name = model.Name;
                    newStatusContent.Status = model.Status;
                    newStatusContent.DateTime = DateTime.UtcNow;
                    newStatusContent.Weight = getLastMinuteContent.Count();
                    newStatusContent = _updateStatusContent.Add(newStatusContent);
                    return View(nameof(Details), newStatusContent);

                }
                else
                {
                    getLastMinuteContent.FirstOrDefault().Weight++;
                    var newStatusContent = _updateStatusContent.Update(getLastMinuteContent.FirstOrDefault());
                    return View(nameof(Details), newStatusContent);
                }
            }
            else
            {
                return NotFound();
            }
        }


        //public List<string> ListBearer()
        //{
        //    var BearerList = _iconfiguration.GetSection("bearer").Get<List<string>>();
        //    return BearerList;
        //}

        public bool IsBearerValid(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            if ((Request.Headers["Authorization"].ToString() ?? "").Trim().Length > 0)
            {
                var _bearer = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var BearerList = _iconfiguration.GetSection("bearer").Get<List<string>>();
                return BearerList.Exists(element => element == _bearer);
            }
            else
            {
                return false;
            }


        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(RestaurantEditModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var newRestaurant = new Restaurant();
        //        newRestaurant.Name = model.Name;
        //        newRestaurant.Cuisine = model.Cuisine;
        //        newRestaurant = _restaurantData.Add(newRestaurant);

        //        // return View("Details",newRestaurant);
        //        return RedirectToAction(nameof(Details), new { id = newRestaurant.Id });
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}
    }
}
