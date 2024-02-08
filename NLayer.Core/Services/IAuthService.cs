using NLayer.Core.Models;

namespace NLayer.Service.Services
{
	public interface IAuthService
	{
		string GenerateTokenString(LoginUser loginUser);
		Task<bool> Login(LoginUser loginUser);
		Task<bool> RegisterUser(LoginUser loginUser);
	}
}