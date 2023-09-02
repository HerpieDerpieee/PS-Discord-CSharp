using Discord;
using Discord.WebSocket;
using Octokit;

public class GithubCommand
{
    public async Task RunCommand(SocketSlashCommand ctx)
    {
        Console.WriteLine("Someone ran the github command");

        string username = (string)ctx.Data.Options.First().Value;

        var github = new GitHubClient(new ProductHeaderValue("DiscordBot"));
        var user = await github.User.Get(username);

        EmbedBuilder embed = new EmbedBuilder();

        embed.WithTitle($"{user.Login}\n{user.Name}");

        embed.WithThumbnailUrl(user.AvatarUrl);

        embed.WithDescription(user.Bio);

        embed.WithUrl(user.HtmlUrl);

        embed.WithColor(Color.DarkBlue);

        embed.AddField("⠀", user.Followers.ToString()+" Followers\n"+user.Following.ToString()+" Following", false);

        embed.AddField("⠀", user.PublicRepos+" public repositories", false);

        await ctx.RespondAsync(embed: embed.Build());
    }


}

