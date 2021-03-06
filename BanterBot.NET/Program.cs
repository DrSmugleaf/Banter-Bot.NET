﻿using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BanterBot.NET.Database.Guilds;
using BanterBot.NET.Dependencies;
using BanterBot.NET.Environments;
using BanterBot.NET.Logging;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Victoria;

namespace BanterBot.NET
{
    internal class Program
    {
        private static void Main()
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly DependencyManager _services;

        private Program()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 50
            });

            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false
            });

            _services = ConfigureServices();

            _client.Log += Logger.LogS;
            _client.Ready += OnReady;
            _client.GuildAvailable += OnGuildAvailable;
            _commands.Log += Logger.LogS;
        }

        private DependencyManager ConfigureServices()
        {
            return new(_client);
        }

        private async Task MainAsync()
        {
            await InitCommands();

            var token = EnvironmentKey.DiscordToken.GetOrThrow();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private async Task InitCommands()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            if (arg is not SocketUserMessage msg) return;
            if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot) return;

            var pos = 0;
            var prefix = "!";

            if (arg.Channel is ITextChannel channel)
            {
                await using var guildContext = new GuildContext();
                var guild = guildContext.Guilds.Single(g => g.Id == channel.GuildId);
                prefix = guild.Prefix ?? "!";
            }

            if (msg.HasStringPrefix(prefix, ref pos) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref pos))
            {
                var context = new SocketCommandContext(_client, msg);
                var result = await _commands.ExecuteAsync(context, pos, _services, MultiMatchHandling.Best);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Logger.ErrorS($"Error executing command: {result.ErrorReason}");
                    await msg.Channel.SendMessageAsync("An unknown error has occurred.");
                }
            }
        }

        private async Task OnReady()
        {
            var lavaNode = _services.GetRequiredService<LavaNode>();

            if (!lavaNode.IsConnected)
            {
                await lavaNode.ConnectAsync();
            }
        }

        private async Task OnGuildAvailable(SocketGuild arg)
        {
            await using var db = new GuildContext();

            if (db.Guilds.Any(guild => guild.Id == arg.Id))
            {
                return;
            }

            db.Add(new Guild {Id = arg.Id});
            await db.SaveChangesAsync();
        }
    }
}
