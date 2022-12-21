using JWTDemo.DBContext;
using JWTDemo.Model;
using JWTDemo.Model.Dto;
using JWTDemo.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JWTDemo.Controllers
{
	[Route("api/Auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private Response _response;
		private readonly ApplicationDBContext _db;

		public AuthController(IAuthService authService, ApplicationDBContext db)
		{
			_authService = authService;
			this._response = new();
			_db = db;
		}

		[HttpPost]
		public async Task<ActionResult<Response>> Register(UserDto userDto)
		{
			try
			{
				if (await _authService.GetUser(userDto.Username) == null)
				{
					User? registeredUser = await _authService.Register(userDto);
					_response.statusCode = HttpStatusCode.OK;
					_response.Result = registeredUser;
				}
				else
				{
					_response.statusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessage = new List<string> { "Username already exists." };
				}
				return Ok(_response);
				
			} catch (Exception ex)
			{
				_response.statusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessage = new List<string> { ex.Message.ToString() };
			}
			return Ok(_response);
		}

		[HttpGet("{username}")]
		public async Task<ActionResult<Response>> GetUser(string username)
		{
			try
			{
				User? user = await _authService.GetUser(username);
				if (user != null)
				{
					_response.statusCode = HttpStatusCode.OK;
					_response.Result = user;
					return Ok(_response);
				}
				else
				{
					_response.statusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessage = new List<string> { "User not found." };
				}
			}
			catch (Exception ex)
			{
				_response.statusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessage = new List<string> { ex.Message.ToString() };
			}
			return Ok(_response);
		}

		[HttpPut]
		public async Task<ActionResult<Response>> Update(UserDto userDto, int id)
		{
			try
			{
				User? user = await _db.Users.AsNoTracking().Where(u => u.Id == id).FirstOrDefaultAsync();
				if (user == null)
				{
					_response.statusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessage = new List<string> { "User not found." };
				}
				else
				{
					User? updatedUser = await _authService.Update(userDto, id);
					_response.statusCode = HttpStatusCode.OK;
					_response.Result = updatedUser;
				}
			}
			catch (Exception ex)
			{
				_response.statusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessage = new List<string> { ex.Message.ToString() };
			}
			return Ok(_response);
		}

		[HttpGet]
		public async Task<ActionResult<Response>> GetUsers()
		{
			try
			{
				IEnumerable<User?> retrievedUsers = await _authService.GetUsers();
				if (retrievedUsers != null)
				{
					_response.statusCode = HttpStatusCode.OK;
					_response.Result = retrievedUsers;
					return Ok(_response);
				}
			}
			catch (Exception ex)
			{
				_response.statusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessage = new List<string> { ex.Message.ToString() };
			}
			return Ok(_response);
		}

		[HttpDelete]
		public async Task<ActionResult<Response>> Delete(string username)
		{
			try
			{
				User? user = await _authService.GetUser(username);
				if (user != null)
				{
					await _authService.Delete(username);
					_response.statusCode = HttpStatusCode.OK;
					_response.Result = user;
					return Ok(_response);
				}
				else
				{
					_response.statusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessage = new List<string> { "User not found." };
				}
			}
			catch (Exception ex)
			{
				_response.statusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessage = new List<string> { ex.Message.ToString() };
			}
			return Ok(_response);
		}
	}
}
