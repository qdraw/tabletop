using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using tabletop.Interfaces;
using tabletop.ViewModels;
using tabletop.Dtos;


namespace tabletop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUpdate _updateStatusContent;

        public HomeController(IUpdate updateStatusContent)
        {
            _updateStatusContent = updateStatusContent;
        }


        public IActionResult Index(DateDto dto)
        {

            var date = dto.GetDateTime();
            var urlSafeName = dto.Name;
            var relativeDays = dto.GetRelativeDays(date);

            var tommorow = date.AddDays(1);
            var yesterday = date.AddDays(-1);

            var allChannelUsers = _updateStatusContent.GetAllChannelUsers()
                .Where(p => p.IsVisible && p.IsAccessible ).ToList();

            // Show default page
            if (string.IsNullOrEmpty(urlSafeName))
            {
                if (allChannelUsers.FirstOrDefault() == null)
                {
                    return BadRequest("Database connection succesfull; Please add a ChannelUser first to continue");
                }

                var find = allChannelUsers.Find(x => x.NameUrlSafe.Contains("tafelvoetbal"));

                if (find != null)
                {
                    urlSafeName = find.NameUrlSafe;
                }
                else
                {
                    var selectItemUrlSafe = allChannelUsers.Single(p => p.NameUrlSafe.Length >= 1).NameUrlSafe;
                    if (selectItemUrlSafe != null)
                    {
                        urlSafeName = selectItemUrlSafe;
                    }
                }

            }

            var channelUserObject = _updateStatusContent.GetChannelUserIdByUrlSafeName(urlSafeName,false);

            if (channelUserObject == null)
            {
                return NotFound("not found");
            }

            var model = new HomeViewModel
            {
                List = allChannelUsers,
                Name = channelUserObject.Name,
                NameId = channelUserObject.NameId,
                NameUrlSafe = channelUserObject.NameUrlSafe,
                RelativeDate = relativeDays,
                Today = date.Year + "-" + dto.LeadingZero(date.Month) + "-" + dto.LeadingZero(date.Day),
                Tomorrow = tommorow.Year + "-"+ dto.LeadingZero(tommorow.Month) + "-" + dto.LeadingZero(tommorow.Day),
                Yesterday = yesterday.Year + "-" + dto.LeadingZero(yesterday.Month) + "-" + dto.LeadingZero(yesterday.Day),
                Day = dto.GetDateTime()
            };

            

            if (model.RelativeDate == 0)
            {
                var isFreeStatus = _updateStatusContent.IsFree(channelUserObject.NameId);
                model.IsFree = isFreeStatus.IsFree;
                model.IsFreeLatestUtcString = isFreeStatus.DateTime.ToString(CultureInfo.InvariantCulture);
                model.IsFreeLatestAmsterdamDateTime = dto.UtcDateTimeToAmsterdamDateTime(isFreeStatus.DateTime);
                return View("Live", model);
            }

            return View("Archive", model);

        }


        //public IActionResult List()
        //{
        //    var model = new RecentStatusClass();
        //    model.RecentStatus = _updateStatusContent.GetAll();

        //    return View(model);
        //}


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
