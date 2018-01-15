using Microsoft.AspNetCore.Mvc;
using tabletop.Interfaces;
using tabletop.Models;

namespace tabletop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUpdateStatus _updateStatusContent;

        public HomeController(IUpdateStatus updateStatusContent)
        {
            _updateStatusContent = updateStatusContent;
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
