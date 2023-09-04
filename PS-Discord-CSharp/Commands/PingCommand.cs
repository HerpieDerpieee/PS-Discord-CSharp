using Discord;
using Discord.WebSocket;
using System.Net.NetworkInformation;

public class PingCommand
{
    DiscordSocketClient bot;

    public PingCommand(DiscordSocketClient bot)
    {
        this.bot = bot;
    }

    public async Task RunCommand(SocketSlashCommand ctx)
    {
        string ping = bot.Latency.ToString() + "ms";

        EmbedBuilder embed = new EmbedBuilder();
        embed.WithTitle("The current ping of the bot is: ");
        embed.WithDescription($"**{ping}**");
        embed.WithColor(Color.Green);

        await ctx.RespondAsync(embed: embed.Build());
    }
}