using Dapper;
using JWTDemo.DBContext;
using JWTDemo.Model;
using JWTDemo.Model.Dto;
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

		public async Task<User> GetUser(string username)
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
			return await connection.QueryFirstAsync("select * from Users where Username = @Username", new { Username = username });
		}

		public async Task<List<User>> GetUsers()
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
			return await connection.QueryFirstAsync("select * from Users");
		}

		public async Task Register(User user)
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultSQLConnection"));
			await connection.ExecuteAsync("inser into Users (Username, PasswordHash, PasswordSalt) values (@Username, @PasswordHash, @PasswordSalt)", user);
		}

		public async Task SaveAsync()
		{

		}
	}
}
