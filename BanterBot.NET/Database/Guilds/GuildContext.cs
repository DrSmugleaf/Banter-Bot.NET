using BanterBot.NET.Environments;
using Microsoft.EntityFrameworkCore;

namespace BanterBot.NET.Database.Guilds
{
    public class GuildContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={EnvironmentKey.PostgresHost.GetOrThrow()};Username={EnvironmentKey.PostgresUser.GetOrThrow()};Password={EnvironmentKey.PostgresPassword.GetOrThrow()};Database={EnvironmentKey.PostgresDb.GetOrThrow()}");
        }
    }
}
