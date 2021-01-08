using BanterBot.NET.Environments;
using Microsoft.EntityFrameworkCore;

namespace BanterBot.NET.Database
{
    public abstract class DatabaseContext : DbContext
    {
        public static bool IsMigration = true;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (IsMigration)
            {
                optionsBuilder.UseNpgsql();
                return;
            }

            optionsBuilder.UseNpgsql($"Host={EnvironmentKey.PostgresHost.GetOrThrow()};Username={EnvironmentKey.PostgresUser.GetOrThrow()};Password={EnvironmentKey.PostgresPassword.GetOrThrow()};Database={EnvironmentKey.PostgresDb.GetOrThrow()}");
        }
    }
}
