using BanterBot.NET.Dependencies;
using Microsoft.EntityFrameworkCore;

namespace BanterBot.NET.Database.Guilds
{
    [Service(ServiceScope.Scoped)]
    public class GuildContext : DatabaseContext
    {
        public DbSet<Guild> Guilds { get; set; } = default!;
    }
}
