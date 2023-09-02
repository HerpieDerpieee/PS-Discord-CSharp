

using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using System.Collections;

public class CommandManager
{
    public async Task RegisterCommands(DiscordSocketClient client)
    {
        List<SlashCommandProperties> commands = new();

        await Console.Out.WriteLineAsync("Creating Commands");

        SlashCommandBuilder githubCommand = new SlashCommandBuilder();
        githubCommand.WithName("github");
        githubCommand.WithDescription("Get information about github stuff");
        githubCommand.AddOption(new SlashCommandOptionBuilder()
            .WithName("user")
            .WithDescription("Lookup someones Github Profile")
            .WithType(ApplicationCommandOptionType.SubCommand)
            .AddOption("username", ApplicationCommandOptionType.String, "The github username of the person you want to see", isRequired: true));

        githubCommand.AddOption(new SlashCommandOptionBuilder()
            .WithName("repository")
            .WithDescription("Lookup a Github Repository")
            .WithType(ApplicationCommandOptionType.SubCommand)
            .AddOption("username", ApplicationCommandOptionType.String, "The github username of the person you want to see", isRequired: true)
            .AddOption("repository-name", ApplicationCommandOptionType.String, "The name of the repository you want to see", isRequired: true));
        commands.Add(githubCommand.Build());

        try
        {
            await client.BulkOverwriteGlobalApplicationCommandsAsync(commands.ToArray());
            await Console.Out.WriteLineAsync("Succesfully Created Commands");


        }
        catch (HttpException exception)
        {
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine(json);
        }
    }

    public async Task SlashCommandHandler(SocketSlashCommand command)
    {
        switch(command.CommandName)
        {
            default:
                await Console.Out.WriteLineAsync("Invalid Command!");
                break;

            case "github":
                GithubCommand cmd = new GithubCommand();
                _ = cmd.RunCommand(command);
                break;
        }
    }
}
