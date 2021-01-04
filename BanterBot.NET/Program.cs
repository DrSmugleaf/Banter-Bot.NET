using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BanterBot.NET.Commands.Music;
using BanterBot.NET.Dependencies;
using BanterBot.NET.Logging;
using BanterBot.NET.Music;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using YoutubeExplode;

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

            _client.Log += Logger.LogS;
            _commands.Log += Logger.LogS;

            _services = ConfigureServices();
        }

        private static DependencyManager ConfigureServices()
        {
            return new();
        }

        private async Task MainAsync()
        {
            await InitCommands();

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
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

            if (msg.HasCharPrefix('!', ref pos) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref pos))
            {
                var context = new SocketCommandContext(_client, msg);
                var result = await _commands.ExecuteAsync(context, pos, _services);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    await msg.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }
    }
}
