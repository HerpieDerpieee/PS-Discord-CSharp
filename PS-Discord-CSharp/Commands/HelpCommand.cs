using Discord.WebSocket;
using Discord;


public class HelpCommand
{
    public async Task RunCommand(SocketSlashCommand ctx)
    {
        EmbedBuilder embed = new EmbedBuilder();
        embed.WithTitle("Help!");
        embed.WithDescription("\n⠀");
        embed.WithColor(Color.DarkBlue);

        //PING HELP SECTION
        embed.AddField("/ping",
                        "Shows you the current latency of the discord bot.\n" +
                        "**Example: /ping**",
                        false
        );

        //GITHUB HELP SECTION
        embed.AddField( "/github user {username}", 
                        "{username} = The GitHub username of the person you want to see the profile of.\n" +
                        "**Example:** /github user HerpieDerpieee\n⠀",
                        false
        );
        embed.AddField( "/github repository {username} {repository name}", 
                        "{username} = The GitHub username of the owner of the repository you want to see\n" +
                        "{repository name} = The name of the repository you want to see.\n" +
                        "**Example:** /github repository microsoft vscode\n⠀",
                        false
        );
        
        //USERINFO HELP SECTION
        embed.AddField("/user-info {user}",
                            "{user} = The discord user you want too see some stuff about\n" +
                            "**Example:** /user-info herpiederpiee",
                            false


        );
        


        await ctx.RespondAsync(embed: embed.Build());
    }
}

