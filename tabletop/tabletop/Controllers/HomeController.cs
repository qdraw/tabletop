using Microsoft.AspNetCore.Mvc;
using tabletop.Interfaces;
//using tabletop.Services;
using tabletop.Models;
//using tabletop.ViewModels;

namespace tabletop.Controllers
{
    public class HomeController : Controller
    {
        private IUpdateStatus _updateStatusContent;

        //private IRestaurantData _restaurantData;
        //private IGreeter _greeter;

        //public HomeController(IRestaurantData restaurantData, IGreeter greeter)
        //{
        //    _restaurantData = restaurantData;
        //    _greeter = greeter;
        //}

        public IActionResult Index()
        {
            //var model = new HomeIndexViewModel();
            //model.Restaurants = _restaurantData.getAll();
            //model.CurrentMessage = _greeter.GetTheMessageOfTheDay();

            //return View(model);
            return View();
        }

        //public IActionResult Details(int id)
        //{
        //    var model = _restaurantData.Get(id);
        //    if (model == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(model);
        //}

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        public HomeController(IUpdateStatus updateStatusContent)
        {
            _updateStatusContent = updateStatusContent;
        }

        [HttpPost]
        public IActionResult Index(UpdateStatus model)
        {
            if (ModelState.IsValid)
            {
                var newStatusContent = new UpdateStatus();
                newStatusContent.Name = model.Name;
                newStatusContent.Status = model.Status;
                newStatusContent = _updateStatusContent.Add(newStatusContent);

                return View("Index", newStatusContent);
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
