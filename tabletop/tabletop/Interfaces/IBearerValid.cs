namespace tabletop.Interfaces
{
	public interface IBearerValid
	{
		bool IsBearerValid(Microsoft.AspNetCore.Http.HttpRequest request, string urlSafeName);
	}
}
