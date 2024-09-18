using NetCord.Services.Commands;
using NetCord.Rest;
using Microsoft.Data.Sqlite;
using NetCord;
using NetCord.Gateway;
using System.Threading.Tasks;
using HutaoWaifuBot.Database;

namespace HutaoWaifuBot.Commands;

public class WaifuCommand : CommandModule<CommandContext>
{
    private readonly string _connectionString = "Data Source=Database/hutaowaifubot.db";

    public WaifuCommand(string connectionString)
    {
        _connectionString = connectionString;
    }

    [Command("waifu")]
    public async Task WaifuAsync()
    {
        var character = GetRandomCharacterFromDatabase();

        EmbedProperties embed = new()
        {
            Title = character.Name,
            Description = $"{character.Series} {(character.Genre == "F" ? "♀️" : character.Genre == "M" ? "♂" : "")}",
            Image = character.Images,
            Color = new(247, 240, 240)
        };

        ButtonProperties button = new("Claim", "Claim", new("❤️"), ButtonStyle.Secondary);

        ActionRowProperties actionRow = new(new[] { button });

        ReplyMessageProperties embedWaifu = new()
        {
            Components = new[] { actionRow },
            AllowedMentions = new AllowedMentionsProperties
            {
                ReplyMention = false
            },
            Embeds = new[] { embed }
        };

        await ReplyAsync(embedWaifu);
    }

    private Character GetRandomCharacterFromDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Name, Series, Genre, Images FROM Characters ORDER BY RANDOM() LIMIT 1";

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Character
            {
                Name = reader.GetString(0),
                Series = reader.GetString(1),
                Genre = reader.GetString(2),
                Images = reader.GetString(3),
            };
        }

        throw new Exception("No characters found in the database.");
    }
}

public class Character
{
    public string Name { get; set; }
    public string Series { get; set; }
    public string Genre { get; set; }
    public string Images { get; set; }
}
