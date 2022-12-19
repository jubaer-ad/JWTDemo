using JWTDemo.Model;
using System.Linq.Expressions;

namespace JWTDemo.Repository
{
	public interface IAuthRepository
	{
		Task Register(User user);
		Task SaveAsync();
		Task<User> GetUser(string username);
	}
}
