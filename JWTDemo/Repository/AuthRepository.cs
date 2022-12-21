using Dapper;
using JWTDemo.Model;
using JWTDemo.Model.Dto;
using Microsoft.Data.SqlClient;

namespace JWTDemo.Repository
{
	public class AuthRepository : IAuthRepository
	{
		//private readonly ApplicationDBContext _db;
		private readonly IConfiguration _config;

		public AuthRepository(IConfiguration config)
		{
			_config = config;
		}

		public async Task<User?> GetUser(string username)
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
			var result = await connection.QueryAsync<User?>("select * from Users where Username = @Username", new { Username = username });
			return result.FirstOrDefault();
		}

		public async Task<IEnumerable<User>> GetUsers()
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
			return await connection.QueryAsync<User>("select * from Users");
		}

		public async Task Register(User user)
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
			await connection.ExecuteAsync("insert into Users (Username, PasswordHash, PasswordSalt) values (@Username, @PasswordHash, @PasswordSalt)", user);
		}

		public async Task Delete(string username)
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
			await connection.ExecuteAsync("DELETE FROM Users WHERE Username = @Username", new { Username = username });
		}

		public async Task Update(User user)
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
			await connection.ExecuteAsync("UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt WHERE Id = @Id)", user);
		}
	}
}
