using Discord.WebSocket;
using Discord;

public class DiscordBot
{
    public static Task Main(string[] args) => new DiscordBot().MainAsync();

    private DiscordSocketClient _client;

    public async Task MainAsync()
    {

        _client = new DiscordSocketClient();

        _client.Ready += OnReady;


        var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private async Task OnReady()
    {
        Console.WriteLine("Bot is ready!");

        CommandManager cmd_mgr = new CommandManager();
        await cmd_mgr.RegisterCommands(_client);
        _client.SlashCommandExecuted += cmd_mgr.SlashCommandHandler;
    }




}