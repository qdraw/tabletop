using Microsoft.AspNetCore.Mvc;
using System;
using tabletop.Interfaces;
//using tabletop.Services;
using tabletop.Models;
//using tabletop.ViewModels;

namespace tabletop.Controllers
{
    public class ApiController : Controller
    {
        private IUpdateStatus _updateStatusContent;

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
        public IActionResult GetLatestByName(string name)
        {
            var model = _updateStatusContent.GetLatestByName(name);
            if (model == null)
            {
                return NotFound();
            }

            return Content(model.DateTime.ToString());
            // return View(model);
        }

        public ApiController(IUpdateStatus updateStatusContent)
        {
            _updateStatusContent = updateStatusContent;
        }


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
            if (ModelState.IsValid)
            {
                var newStatusContent = new UpdateStatus();
                newStatusContent.Name = model.Name;
                newStatusContent.Status = model.Status;
                newStatusContent.DateTime = DateTime.UtcNow;
                newStatusContent = _updateStatusContent.Add(newStatusContent);

                //return RedirectToAction(nameof(Details), new { id = newStatusContent.Id });

                return View(nameof(Details), newStatusContent);
            }
            else
            {
                return View();
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
