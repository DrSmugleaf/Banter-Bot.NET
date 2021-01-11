using System;
using System.Reflection;
using BanterBot.NET.Dependencies;
using BanterBot.NET.Logging;
using Microsoft.EntityFrameworkCore;

namespace BanterBot.NET.Database
{
    [Service]
    public class DatabaseService
    {
        // Default to true to avoid errors when creating migrations due to lack of environment variables
        public static bool IsMigration = true;

        public DatabaseService()
        {
            IsMigration = false;
            MigrateAllContexts();
        }

        private void MigrateAllContexts()
        {
            foreach (var type in Assembly.GetExecutingAssembly().DefinedTypes)
            {
                if (!type.IsSubclassOf(typeof(DbContext)) ||
                    type.IsAbstract)
                {
                    continue;
                }

                Logger.DebugS($"Creating instance of {type} for migration.");

                var instance = (DbContext) (Activator.CreateInstance(type) ?? throw new InvalidOperationException());

                instance.Database.Migrate();
            }
        }
    }
}
