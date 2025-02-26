// WSC/Data/DbInitializer.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

            bool needsMigration = false;

            // Check if tables exist
            try
            {
                // Check if Cards table has any content
                int cardCount = await context.Cards.CountAsync();
                logger.LogInformation($"Found {cardCount} cards in database.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("no such table: Cards"))
                {
                    logger.LogWarning("Cards table doesn't exist. Schema needs to be created.");
                    needsMigration = true;
                }
            }

            try
            {
                // Check if Users table exists
                int userCount = await context.Users.CountAsync();
                logger.LogInformation($"Found {userCount} users in database.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("no such table: Users"))
                {
                    logger.LogWarning("Users table doesn't exist. Schema needs to be updated.");
                    needsMigration = true;
                }
            }

            try
            {
                // Check if CollectionItems table exists
                int collectionCount = await context.CollectionItems.CountAsync();
                logger.LogInformation($"Found {collectionCount} collection items in database.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("no such table: CollectionItems"))
                {
                    logger.LogWarning("CollectionItems table doesn't exist. Schema needs to be updated.");
                    needsMigration = true;
                }
            }

            if (needsMigration)
            {
                logger.LogInformation("Creating missing database tables...");

                // First, save card data if it exists
                List<Card> existingCards = new List<Card>();
                try
                {
                    existingCards = await context.Cards.ToListAsync();
                    logger.LogInformation($"Backed up {existingCards.Count()} existing cards before schema update.");
                }
                catch { /* Ignore if Cards table doesn't exist */ }

                // Create missing tables
                await CreateMissingTables(context, logger);

                // If we had existing cards and the Cards table was recreated, restore them
                if (existingCards.Count() > 0)
                {
                    try
                    {
                        // Check if the Cards table is now empty
                        if (await context.Cards.CountAsync() == 0)
                        {
                            logger.LogInformation("Restoring backed up cards...");
                            await context.Cards.AddRangeAsync(existingCards);
                            await context.SaveChangesAsync();
                            logger.LogInformation($"Successfully restored {existingCards.Count()} cards.");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error restoring card data.");
                    }
                }
            }

            logger.LogInformation("Database initialization completed.");
        }

        private static async Task CreateMissingTables(AppDbContext context, ILogger logger)
        {
            try
            {
                // Execute SQL to create missing tables directly
                var connection = context.Database.GetDbConnection();
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    // Create Users table if it doesn't exist
                    command.CommandText = @"
CREATE TABLE IF NOT EXISTS Users (
    Id TEXT PRIMARY KEY,
    Username TEXT NOT NULL,
    Email TEXT NOT NULL,
    PasswordHash TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    LastLogin TEXT
);
CREATE UNIQUE INDEX IF NOT EXISTS IX_Users_Username ON Users(Username);
CREATE UNIQUE INDEX IF NOT EXISTS IX_Users_Email ON Users(Email);
";
                    await command.ExecuteNonQueryAsync();

                    // Create CollectionItems table if it doesn't exist
                    command.CommandText = @"
CREATE TABLE IF NOT EXISTS CollectionItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CardId INTEGER NOT NULL,
    UserId TEXT,
    GuestId TEXT,
    Quantity INTEGER NOT NULL DEFAULT 1,
    AddedDate TEXT NOT NULL,
    Notes TEXT,
    FOREIGN KEY (CardId) REFERENCES Cards(CardId) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS IX_CollectionItems_CardId ON CollectionItems(CardId);
CREATE INDEX IF NOT EXISTS IX_CollectionItems_UserId_CardId ON CollectionItems(UserId, CardId);
CREATE INDEX IF NOT EXISTS IX_CollectionItems_GuestId_CardId ON CollectionItems(GuestId, CardId);
";
                    await command.ExecuteNonQueryAsync();
                }

                logger.LogInformation("Missing tables created successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating missing tables.");
                throw; // Rethrow to be handled by the caller
            }
        }
    }
}