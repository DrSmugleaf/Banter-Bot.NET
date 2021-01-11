using System;
using BanterBot.NET.Environments;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BanterBot.NET.Database
{
    public abstract class DatabaseContext : DbContext
    {

        private NpgsqlConnectionStringBuilder GetConnectionString()
        {
            if (EnvironmentKey.DatabaseUrl.TryGet(out var url))
            {
                if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                {
                    throw new InvalidOperationException("DatabaseUrl environment variable is not a valid absolute URI");
                }

                var userInfo = uri.UserInfo.Split(':');

                return new NpgsqlConnectionStringBuilder
                {
                    Host = uri.Host,
                    Port = uri.Port,
                    Username = userInfo[0],
                    Password = userInfo[1],
                    Database = uri.LocalPath.TrimStart('/')
                };
            }

            return new NpgsqlConnectionStringBuilder
            {
                Host = EnvironmentKey.PostgresHost.GetOrThrow(),
                Username = EnvironmentKey.PostgresUser.GetOrThrow(),
                Password = EnvironmentKey.PostgresPassword.GetOrThrow(),
                Database = EnvironmentKey.PostgresDb.GetOrThrow()
            };
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (DatabaseService.IsMigration)
            {
                optionsBuilder.UseNpgsql();
                return;
            }

            optionsBuilder.UseNpgsql(GetConnectionString().ConnectionString);
        }
    }
}
