// WSC/Data/DbInitializer.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WSC.Models;

namespace WSC.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(AppDbContext context, ILogger logger)
        {
            logger.LogInformation("Starting database initialization...");

            // Ensure database is created
            bool wasCreated = await context.Database.EnsureCreatedAsync();
            if (wasCreated)
            {
                logger.LogInformation("Database was created.");
            }
            else
            {
                logger.LogInformation("Database already exists.");
            }

            // Check if tables exist
            try
            {
                // Check if Cards table has any content
                int cardCount = await context.Cards.CountAsync();
                logger.LogInformation($"Found {cardCount} cards in database.");

                // Check if Users table exists
                int userCount = await context.Users.CountAsync();
                logger.LogInformation($"Found {userCount} users in database.");

                // Check if CollectionItems table exists
                int collectionCount = await context.CollectionItems.CountAsync();
                logger.LogInformation($"Found {collectionCount} collection items in database.");
            }
            catch (Exception ex)
            {
                // If we get here, it might be because one of the tables doesn't exist
                logger.LogError(ex, "Error checking database tables. Attempting to recreate database...");

                // Try to recreate the database
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                logger.LogInformation("Database has been recreated.");
            }

            logger.LogInformation("Database initialization completed.");
        }
    }
}