using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tabletop.Interfaces;
using tabletop.Models;

namespace tabletop.Controllers
{
	public class ActivityController : Controller
	{
		private readonly IActivityUpdate _activityUpdate;
		private readonly IBearerValid _bearerValid;

		public ActivityController(IActivityUpdate activityUpdate, IBearerValid bearerValid)
		{
			_activityUpdate = activityUpdate;
			_bearerValid = bearerValid;
		}
		
		[HttpPost]
		[Produces("application/json")]
		public async Task<IActionResult> Add(InputChannelActivity inputModel)
		{

			if (!ModelState.IsValid) return BadRequest("Model is incomplete");

			var bearerValid = _bearerValid.IsBearerValid(Request,inputModel.EventName);
			if (!bearerValid) return BadRequest("Authorisation Error");

			
			var model = new ChannelActivity
			{
				// missing channelId, will added later
				Description = inputModel.Description,
				Success = inputModel.Success,
				TimeSpan = inputModel.TimeSpan,
				DateTime = inputModel.DateTime
			};
			
			await _activityUpdate.Add(model,inputModel.EventName);
			
			return Ok();
		}
	}
}
