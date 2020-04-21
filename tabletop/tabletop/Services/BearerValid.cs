using tabletop.Interfaces;

namespace tabletop.Services
{
	public class BearerValid : IBearerValid
	{
		private  readonly IUpdate _updateStatusContent;

		public BearerValid(IUpdate updateStatusContent)
		{
			_updateStatusContent = updateStatusContent;
		}
		
		public bool IsBearerValid(Microsoft.AspNetCore.Http.HttpRequest request, string urlSafeName)
		{
			if ( request.Query.ContainsKey("Bearer") )
			{
				request.Query.TryGetValue("Bearer", out var bearerStringValues);
				return ValidateBearer(urlSafeName, bearerStringValues);
			}
			
			if ( ( request.Headers["Authorization"].ToString() ?? "" ).Trim().Length <= 0 )
				return false;
	        
			var bearer = request.Headers["Authorization"]
				.ToString().Replace("Bearer ", "");
			return ValidateBearer(urlSafeName, bearer);
		}

		private bool ValidateBearer(string urlSafeName, string bearer)
		{
			var channelUser = _updateStatusContent
				.GetChannelUserIdByUrlSafeName(urlSafeName,true);
			return channelUser.Bearer == bearer;
		}
	}
}
