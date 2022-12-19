using System.Net;

namespace JWTDemo.Model
{
	public class Response
	{
		public HttpStatusCode statusCode { get; set; }
		public bool IsSuccess { get; set; } = true;
		public List<string>? ErrorMessage { get; set; }
		public object? Result { get; set; }
	}
}
