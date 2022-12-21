using JWTDemo.Model;
using JWTDemo.Model.Dto;
using JWTDemo.Repository;
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


		public async Task<User?> Register(UserDto userDto)
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

		public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512())
			{
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				passwordSalt = hmac.Key;
			}
		}

		public async Task<User?> GetUser(string username)
		{
			return await _authRepository.GetUser(username);
		}

		public async Task<IEnumerable<User?>> GetUsers()
		{
			return await _authRepository.GetUsers();
		}

		public async Task Delete(string username)
		{
			await _authRepository.Delete(username);
		}

		public async Task<User> Update(UserDto userDto, int id)
		{
			CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
			User user = new()
			{
				Username = userDto.Username,
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt
			};
			await _authRepository.Update(user, id);
			return user;
		}
	}
}
