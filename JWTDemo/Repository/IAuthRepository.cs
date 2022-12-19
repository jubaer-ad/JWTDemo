using JWTDemo.Model;

namespace JWTDemo.Repository
{
	public interface IAuthRepository
	{
		Task Register(User user);
		Task SaveAsync();
	}
}
