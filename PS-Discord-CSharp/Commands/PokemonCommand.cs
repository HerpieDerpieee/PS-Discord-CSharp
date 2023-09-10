using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PokemonCommand
{
    private Random random = new Random();
    public async Task RunCommand(SocketSlashCommand ctx)
    {
        var type = ctx.Data.Options.First().Name;

        switch (type)
        {
            default:
                Console.WriteLine("[ ERROR ] Invalid type passed trough");
                break;
            
            case "catch":
                int randomID = random.Next(1, 1010);
                PokemonData pokemon = await getRandomPokemon(randomID);


                string TypesString = "";
                foreach (String pokemonType in pokemon.Types)
                {
                    TypesString += Capitalize(pokemonType) + "\n";
                }
                
                var embed = new EmbedBuilder()
                    .WithTitle("YOU CAUGHT A POKEMON!")
                    .AddField("Name: ", Capitalize(pokemon.Name), true)
                    .AddField("ID: ", randomID, true)
                    .AddField("Types: ", TypesString, false)


                    .WithImageUrl(pokemon.ImageUrl)

                    .WithColor(Color.Green);

                await ctx.RespondAsync(embed: embed.Build());
                
                break;
        }
    }
    
    private async Task<PokemonData> getRandomPokemon(int id)
    {

        using (HttpClient httpClient = new HttpClient())
        {
            // Make an API request to fetch Pokémon data
            string apiUrl = $"https://pokeapi.co/api/v2/pokemon/{id}/";
            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(content);
                
                var pokemonData = new PokemonData
                {
                    Name = json["name"].ToString(),
                    Types = json["types"].Select(type => type["type"]["name"].ToString()).ToList()
                };
                pokemonData.ImageUrl = json["sprites"]["other"]["official-artwork"]["front_default"].ToString();

                return pokemonData;
            }
            throw new Exception($"Failed to fetch Pokémon data: {response.StatusCode}");
        }
    }
    
    public string Capitalize(string input) {
        string result = char.ToUpper(input[0]) + input.Substring(1);
        return result;
    }
}

public class PokemonData
{
    public string Name { get; set; }
    public List<string> Types { get; set; }
    public string ImageUrl { get; set; }
}

