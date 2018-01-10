using Microsoft.AspNetCore.Mvc;
using System;
using tabletop.Interfaces;
//using tabletop.Services;
using tabletop.Models;
//using tabletop.ViewModels;

namespace tabletop.Controllers
{
    public class HomeController : Controller
    {
        private IUpdateStatus _updateStatusContent;

        public HomeController(IUpdateStatus updateStatusContent)
        {
            _updateStatusContent = updateStatusContent;
        }

        public IActionResult List()
        {
            var model = new RecentStatusClass();
            model.RecentStatus = _updateStatusContent.getAll();

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
