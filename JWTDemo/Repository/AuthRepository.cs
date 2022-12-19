using JWTDemo.DBContext;
using JWTDemo.Model;
using System.Data.Entity;
using System.Linq.Expressions;

namespace JWTDemo.Repository
{
	public class AuthRepository : IAuthRepository
	{
		private readonly ApplicationDBContext _db;

		public AuthRepository(ApplicationDBContext db)
		{
			_db = db;
		}

		public async Task<User> GetUser(Expression<Func<User, bool>>? filter, bool tracked = false)
		{
			IQueryable<User> query = _db.Set<User>();
			if (!tracked) query = query.AsNoTracking();
			if(filter != null)
			{
				query = query.Where(filter);
			}
			return await query.FirstOrDefaultAsync();
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
