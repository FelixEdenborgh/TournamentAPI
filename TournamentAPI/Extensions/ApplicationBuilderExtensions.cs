using Tournament.Data.Data;
using TournamentData.Data.Data;

namespace Tournament.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TournamentApiContext>();
                await SeedData.SeedDatabaseAsync(context);
            }
        }
    }
}
