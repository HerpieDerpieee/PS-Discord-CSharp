using Discord.WebSocket;
using Discord;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HelpCommand
{
    public async Task RunCommand(SocketSlashCommand ctx)
    {
        EmbedBuilder embed = new EmbedBuilder();
        embed.WithTitle("Help!");
        embed.WithDescription("\n⠀");
        embed.WithColor(Color.DarkBlue);

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


        await ctx.RespondAsync(embed: embed.Build());
    }
}

