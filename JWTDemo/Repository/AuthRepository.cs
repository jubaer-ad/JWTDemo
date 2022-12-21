using Dapper;
using JWTDemo.DBContext;
using JWTDemo.Model;
using JWTDemo.Model.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace JWTDemo.Repository
{
	public class AuthRepository : IAuthRepository
	{
		//private readonly ApplicationDBContext _db;
		private readonly IConfiguration _config;
		private readonly ApplicationDBContext _db;

		public AuthRepository(IConfiguration config, ApplicationDBContext db)
		{
			_config = config;
			_db = db;
		}

		public async Task<User?> GetUser(string username)
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
			var result = await connection.QueryAsync<User?>("SELECT * FROM Users WHERE Username = @Username", new { Username = username });
			return result.FirstOrDefault();
		}

		public async Task<IEnumerable<User>> GetUsers()
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
			return await connection.QueryAsync<User>("SELECT * FROM Users");
		}

		public async Task Register(User user)
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
			await connection.ExecuteAsync("INSERT INTO Users (Username, PasswordHash, PasswordSalt) VALUES (@Username, @PasswordHash, @PasswordSalt)", user);
		}

		public async Task Delete(string username)
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
			await connection.ExecuteAsync("DELETE FROM Users WHERE Username = @Username", new { Username = username });
		}

		public async Task<User?> Update(User user, int id)
		{
			User? userToBeUpdated = await _db.Users.AsNoTracking().Where(u => u.Id == id).FirstOrDefaultAsync();
			if (userToBeUpdated != null)
			{
				using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
				await connection.ExecuteAsync("UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash," +
					" PasswordSalt = @PasswordSalt WHERE Id = @Id", new { user.Username, user.PasswordHash, 
						user.PasswordSalt, Id = id });
				return await GetUser(user.Username);
			}
			return null;
		}
	}
}
