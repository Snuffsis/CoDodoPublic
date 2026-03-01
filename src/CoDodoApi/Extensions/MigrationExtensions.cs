using CoDodoApi.Database;
using Microsoft.EntityFrameworkCore;

namespace CoDodoApi.Extensions;

/// <summary>
/// Extension methods for migrations.
/// </summary>
public static class MigrationExtensions
{
	/// <summary>
	/// Apply any pending migrations.
	/// </summary>
	/// <param name="app">The app it will apply migrations for</param>
	public static void ApplyMigrations(this IApplicationBuilder app)
	{
		using IServiceScope scope = app.ApplicationServices.CreateScope();

		using ApplicationDbContext dbContext =
			scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		dbContext.Database.Migrate();
	}
}