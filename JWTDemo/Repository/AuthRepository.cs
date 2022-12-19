using Dapper;
using JWTDemo.DBContext;
using JWTDemo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.Entity;
using System.Linq.Expressions;

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

		[HttpGet("{username}")]
		public async Task<User> GetUser(string username)
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
			return await connection.QueryFirstAsync("select * from Users where Username = @Username", new { Username = username });
		}

		public async Task Register(User user)
		{

		}

		public async Task SaveAsync()
		{

		}
	}
}
