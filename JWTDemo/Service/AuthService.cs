using JWTDemo.DBContext;
using JWTDemo.Model;
using JWTDemo.Model.Dto;
using JWTDemo.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Cryptography;

namespace JWTDemo.Service
{
	public class AuthService : IAuthService
	{
		private readonly IAuthRepository _authRepository;
		public AuthService(IAuthRepository authRepository)
		{
			_authRepository = authRepository;
		}


		public async Task<User> Register(UserDto userDto)
		{
			if (_authRepository.GetUser(userDto.Username) == null)
			{
				CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
				User user = new()
				{
					Username = userDto.Username,
					PasswordHash = passwordHash,
					PasswordSalt = passwordSalt
				};
				await _authRepository.Register(user);
				return user;
			}
			return "Username already exists!";
			
		}

		public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512())
			{
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				passwordSalt = hmac.Key;
			}
		}

		public async Task<User> GetUser(string username)
		{
			
		}
	}
}
