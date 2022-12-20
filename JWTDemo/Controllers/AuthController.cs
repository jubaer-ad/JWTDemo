using Azure.Core;
using JWTDemo.Model;
using JWTDemo.Model.Dto;
using JWTDemo.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JWTDemo.Controllers
{
	[Route("api/Auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private Response _response;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
			this._response = new();
		}

		[HttpPost]
		public async Task<ActionResult<Response>> Register(UserDto request)
		{
			try
			{
				if (_authService.GetUser(request.Username) == null)
				{
					User? user = await _authService.Register(request);
					_response.statusCode = HttpStatusCode.OK;
					_response.Result = user;
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
		public async Task<User> GetUser(string username)
		{
			try
			{
				User user = await _authService.GetUser(username);
				if (user != null) { P}
				_response.statusCode = HttpStatusCode.OK;
				_response.Result = user;
				return Ok(_response);


				if (_authService.GetUser(request.Username) == null)
				{
					User? user = await _authService.Register(request);
					_response.statusCode = HttpStatusCode.OK;
					_response.Result = user;
				}
				else
				{
					_response.statusCode = HttpStatusCode.BadRequest;
					_response.IsSuccess = false;
					_response.ErrorMessage = new List<string> { "Username already exists." };
				}
				return Ok(_response);

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
