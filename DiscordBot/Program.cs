using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;


public class Author
{
    private SocketUser _user;

    public Author(SocketUser user)
    {
        _user = user;
    }

    public ulong Id => _user.Id;
    public string Username => _user.Username;
    public string Mention => _user.Mention;

    public override string ToString()
    {
        return $"{Username}#{_user.Discriminator}";
    }
    private async Task HandleCommandAsync(SocketMessage arg)
    {
        var message = arg as SocketUserMessage;
        var context = new SocketCommandContext(_client, message);

        if (context.Author.IsBot) return;

        if (message.Content.ToLower() == "!ping")
        {
            var author = new Author(context.Author);
            await context.Channel.SendMessageAsync($"Pong, {author.Mention}!");
        }
    }
}
class Program : Author
{
    private DiscordSocketClient _client;

    static void Main(string[] args)
    {
        new Program().RunBotAsync().GetAwaiter().GetResult();
    }

    public async Task RunBotAsync()
    {
        _client = new DiscordSocketClient();

        _client.Log += Log;

        await RegisterCommandsAsync();

        await _client.LoginAsync(TokenType.Bot, "YOUR_BOT_TOKEN");
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private Task Log(LogMessage arg)
    {
        Console.WriteLine(arg);
        return Task.CompletedTask;
    }

    public async Task RegisterCommandsAsync()
    {
        _client.MessageReceived += HandleCommandAsync;
        await Task.CompletedTask;
    }

    private async Task HandleCommandAsync(SocketMessage arg)
    {
        var message = arg as SocketUserMessage;
        var context = new SocketCommandContext(_client, message);

        if (context.Author.IsBot) return;

        if (message.Content.ToLower() == "!ping")
        {
            await context.Channel.SendMessageAsync("Pong!");
        }
    }
}
