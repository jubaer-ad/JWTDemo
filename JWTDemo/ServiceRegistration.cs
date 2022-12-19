using JWTDemo.Repository;
using JWTDemo.Service;

namespace JWTDemo
{
	public static class ServiceRegistration
	{
		public static void AddCustomeServices(this IServiceCollection services)
		{
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IAuthRepository, AuthRepository>();
		}
	}
}
