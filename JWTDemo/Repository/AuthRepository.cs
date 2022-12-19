using JWTDemo.DBContext;
using JWTDemo.Model;

namespace JWTDemo.Repository
{
	public class AuthRepository : IAuthRepository
	{
		private readonly ApplicationDBContext _db;

		public AuthRepository(ApplicationDBContext db)
		{
			_db = db;
		}
		public async Task Register(User user)
		{
			await _db.Users.AddAsync(user);
			await SaveAsync();
		}

		public async Task SaveAsync()
		{
			await _db.SaveChangesAsync();
		}
	}
}
