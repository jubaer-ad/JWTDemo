using JWTDemo.Model;
using Microsoft.EntityFrameworkCore;

namespace JWTDemo.DBContext
{
	public class ApplicationDBContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> opt) : base(opt) { }

	}
}
