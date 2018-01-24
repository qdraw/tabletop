using System.Linq;
using Microsoft.AspNetCore.Mvc;
using tabletop.Interfaces;
using tabletop.Models;
using tabletop.ViewModels;
using tabletop.Dtos;


namespace tabletop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUpdateStatus _updateStatusContent;

        public HomeController(IUpdateStatus updateStatusContent)
        {
            _updateStatusContent = updateStatusContent;
        }

        public IActionResult Index(DateDto dto)
        {
            var date = dto.GetDateTime();
            var name = dto.Name;
            var relativeDays = dto.GetRelativeDays(date);

            var tommorow = date.AddDays(1);
            var yesterday = date.AddDays(-1);


            var model = new HomeViewModel
            {
                List = _updateStatusContent.GetUniqueNames(),
                Name = name,
                RelativeDate = relativeDays,
                Today = date.Year + "-" + dto.LeadingZero(date.Month) + "-" + dto.LeadingZero(date.Day),
                Tomorrow = tommorow.Year + "-"+ dto.LeadingZero(tommorow.Month) + "-" + dto.LeadingZero(tommorow.Day),
                Yesterday = yesterday.Year + "-" + dto.LeadingZero(yesterday.Month) + "-" + dto.LeadingZero(yesterday.Day),
                Day = dto.GetDateTime()
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

            if (model.RelativeDate == 0)
            {
                model.IsFree = _updateStatusContent.IsFree(name).IsFree;
                model.IsFreeDateTime = _updateStatusContent.IsFree(name).DateTime;
                return View("Live", model);
            }

            return View("Archive", model);

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
