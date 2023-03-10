using JWTDemo.Model.Dto;
using JWTDemo.Model;
using Microsoft.AspNetCore.Mvc;

namespace JWTDemo.Service
{
	public interface IAuthService
	{
		Task<User?> Register(UserDto userDto);
		void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
		Task<User?> GetUser(string username);
		Task<IEnumerable<User?>> GetUsers();
		Task Delete(string username);
		Task<User?> Update(UserDto userDto, int id);
		Task<string?> Login(UserDto userDto);
	}
}
