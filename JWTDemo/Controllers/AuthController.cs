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
				User user = await _authService.Register(request);
				_response.statusCode = HttpStatusCode.OK;
				_response.Result = user;
			} catch (Exception ex)
			{
				_response.statusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessage = new List<string> { ex.Message.ToString() };
			}
			return Ok(_response);
		}
	}
}
