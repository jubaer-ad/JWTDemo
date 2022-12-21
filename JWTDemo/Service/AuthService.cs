using Azure;
using JWTDemo.Model;
using JWTDemo.Model.Dto;
using JWTDemo.Repository;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace JWTDemo.Service
{
	public class AuthService : IAuthService
	{
		private readonly IAuthRepository _authRepository;
		private readonly string _secretKey;

		public AuthService(IAuthRepository authRepository, IConfiguration config)
		{
			_authRepository = authRepository;
			_secretKey = config.GetValue<string>("AppSecurity:Secret") ?? "default_at_least_sixteen_chars_secret_key";
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

		public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using var hamc = new HMACSHA512(passwordSalt);
			var computedPasswordHash = hamc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			return computedPasswordHash == passwordHash;
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

		public async Task<User?> Update(UserDto userDto, int id)
		{
			CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
			User user = new()
			{
				Username = userDto.Username,
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt
			};
			User? updatedUser = await _authRepository.Update(user, id);
			return updatedUser;
		}

		public async Task<string?> Login(UserDto userDto)
		{
			User? user = await _authRepository.GetUser(userDto.Username);
			if (user == null)
				return "User not found.";
			if (!VerifyPassword(userDto.Password, user.PasswordHash, user.PasswordSalt))
				return "Wrong password.";
			return CreateToken(user);
		}

		private string? CreateToken(User user)
		{
			List<Claim> claims = new()
			{
				new Claim(ClaimTypes.Name, user.Username)
			};
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
			var token = 
		}
	}
}
