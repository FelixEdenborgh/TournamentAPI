using Microsoft.EntityFrameworkCore;
using TournamentData.Data.Data;
using TournamentCore.Core.Entities;

namespace Tournament.Data.Data
{
    public static class SeedData
    {
        public static async Task SeedDatabaseAsync(TournamentApiContext context)
        {
            // Kontrollera om databasen redan har seed-data
            if (!await context.Tournaments.AnyAsync())
            {
                var tournament = new TournamentEntities
                {
                    Title = "Summer Tournament",
                    StartDate = new DateTime(2024, 6, 1),
                    Games = new List<GameEntities>
                    {
                        new GameEntities
                        {
                            Title = "Game 1",
                            Time = new DateTime(2024, 6, 2)
                        },
                        new GameEntities
                        {
                            Title = "Game 2",
                            Time = new DateTime(2024, 6, 3)
                        }
                    }
                };

                await context.Tournaments.AddAsync(tournament);
                await context.SaveChangesAsync();
            }
        }
    }
}
