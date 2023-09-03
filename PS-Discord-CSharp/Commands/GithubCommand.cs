using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


public class GithubCommand
{
    public async Task RunCommand(SocketSlashCommand ctx)
    {
        var type = ctx.Data.Options.First().Name;


        switch (type)
        {
            default:
                Console.WriteLine("Invalid Type");
                break;

            case "user":
                string username = (string) ctx.Data.Options.FirstOrDefault(option => option.Name == "user")?.Options.FirstOrDefault(option => option.Name == "username").Value;

                JObject response = await GetJObjectAsync($"users/{username}");
                if (response == null)
                {
                    await ctx.RespondAsync("The selected user was not found, please check your spelling or try again later");
                    return;
                }
                EmbedBuilder embed = new EmbedBuilder();
                embed.WithUrl(response.GetValue("html_url").ToString());
                embed.WithTitle($"{response.GetValue("login")}\n{response.GetValue("name")}");
                embed.WithDescription(response.GetValue("bio").ToString());
                embed.WithThumbnailUrl(response.GetValue("avatar_url").ToString());
                embed.WithColor(Color.DarkBlue);

                embed.AddField("⠀", response.GetValue("followers").ToString() + " Followers\n" + response.GetValue("following").ToString() + " Following", false);
                embed.AddField("⠀", response.GetValue("public_repos").ToString() + " public repositories", false);


                await ctx.RespondAsync(embed:embed.Build());
                break;

            case "repository":
                string repo_username = (string)ctx.Data.Options.FirstOrDefault(option => option.Name == "repository")?.Options.FirstOrDefault(option => option.Name == "username").Value;
                string repo_repository_name = (string)ctx.Data.Options.FirstOrDefault(option => option.Name == "repository")?.Options.FirstOrDefault(option => option.Name == "repository-name").Value;

                JObject repo_response = await GetJObjectAsync($"repos/{repo_username}/{repo_repository_name}");
                JObject commit_response = await GetJObjectAsync($"repos/{repo_username}/{repo_repository_name}/commits");
                
                if (repo_response == null)
                {
                    await ctx.RespondAsync("The selected repository was not found, please check your spelling or try again later");
                    return;
                }
                else if (commit_response == null) {
                    await ctx.RespondAsync("There where no commits found in the selected repository, please check your spelling or try again later");
                    return;
                }

                JObject commitData = (JObject) commit_response.GetValue("commit");
                JObject commitUserData = (JObject)commitData.GetValue("committer");

                string committerName = commitUserData.GetValue("name").ToString();
                string committerEmail = commitUserData.GetValue("email").ToString();
                string committedMessage = commitData.GetValue("message").ToString();

                EmbedBuilder repo_embed = new EmbedBuilder();
                repo_embed.WithUrl(repo_response.GetValue("html_url").ToString());
                repo_embed.WithTitle(repo_response.GetValue("full_name").ToString());
                repo_embed.WithDescription("Most Used Language: "+repo_response.GetValue("language").ToString());
                repo_embed.WithColor(Color.DarkBlue);

                repo_embed.AddField("⠀", $"Stargazer Count: {repo_response.GetValue("stargazers_count")}", false);
                repo_embed.AddField("⠀", $"Open Issues: {repo_response.GetValue("open_issues")}", false);
                
                repo_embed.AddField("⠀", "⠀", false);
                repo_embed.AddField("Latest Commit: ", $"{committerName}  ({committerEmail})\n{committedMessage}", false);

                repo_embed.WithFooter($"Estimated Repository Size: {repo_response.GetValue("size")}Kb");

                await ctx.RespondAsync(embed: repo_embed.Build());
                break;
        }

    }
    public async Task<JObject> GetJObjectAsync(string content)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "CSharpDiscordBot");

            HttpResponseMessage response = await httpClient.GetAsync($"https://api.github.com/{content}");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                if (json.StartsWith("["))
                {
                    JArray jsonArray = JArray.Parse(json);
                    if (jsonArray.Count > 0)
                    {
                        return jsonArray[0] as JObject;
                    }
                    return null;
                }
                else
                {
                    JObject userProfile = JsonConvert.DeserializeObject<JObject>(json);
                    return userProfile;
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return null;
            }
        }
    }
}

