using System;
using System.Collections.Generic;
using System.Linq;
using Discord;

namespace BanterBot.NET.Extensions
{
    public static class GuildUserExtensions
    {
        public static IEnumerable<IRole> GetRoles(this IGuildUser user)
        {
            return user.RoleIds.Select(id => user.Guild.GetRole(id));
        }

        public static IRole? GetHighestRole(this IGuildUser user, Func<IRole, bool>? filter)
        {
            filter ??= _ => true;

            var filteredRoles = user.GetRoles().Where(filter).ToArray();

            if (!filteredRoles.Any())
            {
                return null;
            }

            return filteredRoles.MaxBy(role => role.Position);
        }

        public static IRole GetHighestRole(this IGuildUser user)
        {
            return user.GetRoles().MaxBy(role => role.Position) ?? user.Guild.EveryoneRole;
        }

        public static bool HasRole(this IGuildUser user, IRole role)
        {
            return user.RoleIds.Contains(role.Id);
        }
    }
}
