using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace ReminderBot
{
    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly ulong _notifyChannelId;
        private readonly string _token;

        public Bot()
        {
            _token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            _notifyChannelId = ulong.Parse(Environment.GetEnvironmentVariable("DISCORD_NOTIFY_CHANNEL_ID"));
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All
            });
            //
            // _client.Log += LogAsync;
            // _client.Ready += OnReadyAsync;
        }
        
        public async Task RunAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _token);
        }
    }
}