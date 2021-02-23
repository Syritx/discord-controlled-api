using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

using api.src.disc.http;

namespace api.src.disc {
    class Bot {

        DiscordSocketClient client;
        CommandService commands;
        IServiceProvider services;

        string prefix, token;

        static void Main(string[] args) => new Bot().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync() {
            
            Https.Start();

            string configFile = "src/disc/config.json";
            string text = File.ReadAllText(configFile);

            JsonDocument doc = JsonDocument.Parse(text);

            token = doc.RootElement.GetProperty("token").ToString();
            prefix = doc.RootElement.GetProperty("prefix").ToString();

            client = new DiscordSocketClient();
            commands = new CommandService();

            services = new ServiceCollection()
                          .AddSingleton(client)
                          .AddSingleton(commands)
                          .BuildServiceProvider();

            client.Log += ClientLog;

            await Commands();
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        public async Task Commands() {
            client.MessageReceived += CommandHandler;
            await commands.AddModulesAsync(System.Reflection.Assembly.GetEntryAssembly(), services);
        }

        async Task CommandHandler(SocketMessage message) {

            var msg = message as SocketUserMessage;
            var commandContext = new SocketCommandContext(client,msg);

            if (message.Author.IsBot) return;

            int argPos = 0;
            if (msg.HasStringPrefix(prefix, ref argPos))
            {
                var result = await commands.ExecuteAsync(commandContext, argPos, services);

                if (!result.IsSuccess) {
                    Console.WriteLine("there was an error: " + result.ErrorReason);
                }
            }
        }

        Task ClientLog(LogMessage message) {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}
