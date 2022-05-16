using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShortenUrl.API.Data.Models;

namespace ShortenUrl.API.Data
{
    public class AppDbContext: DbContext
	{
		public DbSet<Link> Links { get; set; }
		public DbSet<User> Users { get; set; }

		public string ConnectionString { get; set; }

		public AppDbContext(
			DbContextOptions options) : base(options)
		{}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "ShortenUrlsDB.db" };
			var connectionString = connectionStringBuilder.ToString();
			var connection = new SqliteConnection(connectionString);

			optionsBuilder.UseSqlite(connection);
		}
	}
}
