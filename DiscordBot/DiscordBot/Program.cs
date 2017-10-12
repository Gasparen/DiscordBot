using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Program
    {
        // TODO: Do a proper check of command line args, this'll crash hard without it
        public static void Main(string[] args) => new Program().MainAsync(args[0]).GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;

        // Private constructor (called from Main above)
        private Program()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });

            _commands = new CommandService();

            _client.Log += Log;
            _commands.Log += Log;
        }

        public async Task MainAsync(String token)
        {
            _client.MessageReceived += MessageReceived;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block task until exited
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            var msg = message as SocketUserMessage;
            if (msg == null)
            {
                await Log(new LogMessage(LogSeverity.Debug, "Message Received", "Message received was NULL"));
                return; // Don't handle system messages (yet)
            }

            int pos = 0;

            if (msg.HasCharPrefix('!', ref pos))
            {
                var context = new SocketCommandContext(_client, msg);
                
                var result = await _commands.ExecuteAsync(context, pos);

                await Log(new LogMessage(LogSeverity.Info, "Command execution", "Resulting in: " + result.IsSuccess.ToString()));

                if (message.Content.Substring(pos,4) == "ping")
                {
                    await message.Channel.SendMessageAsync("Pong!");
                    await Log(new LogMessage(LogSeverity.Info, "Message Received - Ping Command", "Ponging channel " + message.Channel.Name));
                }
                else if (message.Content.Substring(pos,7) == "command")
                {
                    await message.Channel.SendMessageAsync(_commands.Commands.ToString());
                    await Log(new LogMessage(LogSeverity.Debug, "Message Received", "Printing list of commands"));
                }
                else
                {
                    await Log(new LogMessage(LogSeverity.Debug, "Message Received", "Command not found: " + message.Content.Substring(pos)));
                }
            }

            var mentionCount = msg.MentionedUsers.Count;

            if (mentionCount > 0)
            {
                await Log(new LogMessage(LogSeverity.Info, "Message Received", "Number of mentions: " + mentionCount));
            }
        }

        private static Task Log(LogMessage message)
        {
            var originalConsoleColor = Console.ForegroundColor;
            switch (message.Severity)
            {
                case LogSeverity.Critical: // Fall Through
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose: // Fall through
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message}");
            Console.ForegroundColor = originalConsoleColor; // Reset console color

            return Task.CompletedTask;
        }
    }
}