using JWTDemo.Model;
using System.Linq.Expressions;

namespace JWTDemo.Repository
{
	public interface IAuthRepository
	{
		Task Register(User user);
		Task SaveAsync();
		Task<User> GetUser(Expression<Func<User>>? filter, bool tracked);
	}
}
