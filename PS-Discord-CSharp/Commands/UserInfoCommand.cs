using Discord;
using Discord.WebSocket;


public class UserInfoCommand
{
    public async Task runCommand(SocketSlashCommand ctx)
    {
        IUser user = (IUser) ctx.Data.Options.FirstOrDefault(option => option.Name == "user").Value;

        string AccountCreationDate = user.CreatedAt.ToString("MMMM dd yyyy")+" at "+user.CreatedAt.ToLocalTime().ToString("HH:mm:ss");
        
        
        
        
        EmbedBuilder embed = new EmbedBuilder();


        if (user.GlobalName == null)
        {
            embed.WithTitle(user.Username);
            embed.WithDescription(user.Username+"#"+user.Discriminator);
            embed.WithThumbnailUrl(user.GetAvatarUrl());
        }
        else
        {
            embed.WithTitle(user.GlobalName);
            embed.WithDescription(user.Username);
            embed.WithThumbnailUrl(user.GetAvatarUrl());
        }

        embed.WithDescription(embed.Description + "\n\n" +$"**Account Creation Date: **\n{AccountCreationDate}");
        embed.AddField("Is Bot: ", $"{user.IsBot.ToString()}", false);
        embed.AddField("Is Webhook: ", $"{user.IsWebhook.ToString()}" , false);
        
        await ctx.RespondAsync(embed: embed.Build());
    }
}
