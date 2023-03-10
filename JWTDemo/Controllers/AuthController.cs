using JWTDemo.Model;
using JWTDemo.Model.Dto;
using JWTDemo.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JWTDemo.Controllers
{
	[Route("api/Auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly Response _response;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
			this._response = new();
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
				User? updatedUser = await _authService.Update(userDto, id);
				if (updatedUser == null)
				{
					_response.statusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessage = new List<string> { "User not found." };
				}
				else
				{
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

		[HttpPost("login")]
		public async Task<ActionResult<Response>> Login(UserDto userDto)
		{
			try
			{
				string? jwt = await _authService.Login(userDto);
				switch (jwt)
				{
					case "0":
						_response.IsSuccess = false;
						_response.ErrorMessage = new List<string> { "User not found." };
						_response.statusCode = HttpStatusCode.BadRequest;
						break;
					case "1":
						_response.IsSuccess = false;
						_response.ErrorMessage = new List<string> { "Wrong password." };
						_response.statusCode = HttpStatusCode.BadRequest;
						break;
					default:
						_response.statusCode = HttpStatusCode.OK;
						_response.Result = jwt;
						break;
				}
			}
			catch (Exception ex)
			{
				_response.statusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessage = new List<string> { ex.Message.ToString() };
			}
			return _response;
		}

	}
}
