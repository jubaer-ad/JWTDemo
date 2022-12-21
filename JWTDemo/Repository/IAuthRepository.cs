using JWTDemo.Model;
using System.Linq.Expressions;

namespace JWTDemo.Repository
{
	public interface IAuthRepository
	{
		Task Register(User user);
		Task<User?> GetUser(string username);
		Task<IEnumerable<User>> GetUsers();
		Task Delete(string username);
		Task Update(User user);
	}
}
