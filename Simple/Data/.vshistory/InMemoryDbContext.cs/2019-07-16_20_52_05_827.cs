using Microsoft.EntityFrameworkCore;

namespace Simple.Data
{
	public class InMemoryDbContext : DbContext
	{
		public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options)
			: base(options)
		{
		}

		public DbSet<Employee> Employees { get; set; }
	}

	public class DataRealTime
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int Price { get; set; }
	}
}
