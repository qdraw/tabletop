using System.Linq;
using Microsoft.AspNetCore.Mvc;
using tabletop.Interfaces;
using tabletop.Models;
using tabletop.ViewModels;

namespace tabletop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUpdateStatus _updateStatusContent;

        public HomeController(IUpdateStatus updateStatusContent)
        {
            _updateStatusContent = updateStatusContent;
        }

        public IActionResult Index(string name, int date)
        {
            if (date <= -1)
            {
                date = date * -1;
            }

            var model = new HomeViewModel
            {
                List = _updateStatusContent.GetUniqueNames(),
                Name = name,
                RelativeDate = date,
                TomorrowRelativeDate = date-1,
                YesterdayRelativeDate = date+1
            };

            if (string.IsNullOrEmpty(name))
            {
                model.Name = "tafelvoetbal";
            }

            var matchNameList = model.List.Where(p => p == model.Name);

            if (!matchNameList.Any() && name != "test")
            {
                return NotFound("not found");
            }

            return View(model);
        }


        public IActionResult List()
        {
            var model = new RecentStatusClass();
            model.RecentStatus = _updateStatusContent.GetAll();

            return View(model);
        }


        //[HttpGet]
        //public IActionResult Update(UpdateStatus model)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var newStatusContent = new UpdateStatus();
        //        newStatusContent.Name = model.Name;
        //        newStatusContent.Status = model.Status;
        //        newStatusContent.DateTime = DateTime.UtcNow;
        //        if(newStatusContent.Status == 1)
        //        {
        //            newStatusContent = _updateStatusContent.Add(newStatusContent);

        //            return Content(newStatusContent.DateTime.ToString());
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }

        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

    }
}
