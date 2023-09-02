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

                JObject response = await GetUserProfileAsync(username);
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

                JObject repo_response = await GetRepositoryAsync(repo_username, repo_repository_name);
                
                if (repo_response == null)
                {
                    await ctx.RespondAsync("The selected repository was not found, please check your spelling or try again later");
                    return;
                }

                EmbedBuilder repo_embed = new EmbedBuilder();
                repo_embed.WithUrl(repo_response.GetValue("html_url").ToString());
                repo_embed.WithTitle(repo_response.GetValue("full_name").ToString());
                repo_embed.WithDescription("Most Used Language: "+repo_response.GetValue("language").ToString());
                repo_embed.WithColor(Color.DarkBlue);

                repo_embed.AddField("⠀", $"Stargazer Count: {repo_response.GetValue("stargazers_count")}", false);
                repo_embed.AddField("⠀", $"Open Issues: {repo_response.GetValue("open_issues")}", false);
                repo_embed.AddField("⠀", $"Estimated Size: around {repo_response.GetValue("size")}Kb", false);

                await ctx.RespondAsync(embed: repo_embed.Build());
                break;
        }

    }
    public async Task<JObject> GetUserProfileAsync(string username)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "CSharpDiscordBot");

            HttpResponseMessage response = await httpClient.GetAsync($"https://api.github.com/users/{username}");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JObject userProfile = JsonConvert.DeserializeObject<JObject>(json);
                return userProfile;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return null;
            }
        }
    }
    public async Task<JObject> GetRepositoryAsync(string username, string repo_name)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "CSharpDiscordBot");

            HttpResponseMessage response = await httpClient.GetAsync($"https://api.github.com/repos/{username}/{repo_name}");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JObject repoDetails = JsonConvert.DeserializeObject<JObject>(json);
                return repoDetails;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return null;
            }
        }
    }
}

