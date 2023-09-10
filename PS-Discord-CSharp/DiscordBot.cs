using Discord.WebSocket;
using Discord;

public class DiscordBot
{
    public static Task Main(string[] args) => new DiscordBot().MainAsync();

    private DiscordSocketClient client;

    private async Task MainAsync()
    {
        Console.WriteLine("Starting Discord Bot...");
        client = new DiscordSocketClient();

        client.Ready += OnReady;


        var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private async Task OnReady()
    {
        Console.WriteLine("Bot is ready!");

        CommandManager cmd_mgr = new CommandManager(client);
        await cmd_mgr.RegisterCommands(client);
        client.SlashCommandExecuted += cmd_mgr.SlashCommandHandler;
    }
}