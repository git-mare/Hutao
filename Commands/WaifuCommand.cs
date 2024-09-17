using NetCord.Gateway;
using NetCord.Rest;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using NetCord;

namespace HutaoWaifuBot.Commands
{
    public class WaifuCommand : ICommand
    {
        private readonly string _connectionString = "Data Source=Data/hutao_waifu_bot.db";

        public WaifuCommand(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task ExecuteAsync(Message message, RestClient client)
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

            MessageProperties embedWaifu = new()
            {
                Components = new[] { actionRow },
                AllowedMentions = new AllowedMentionsProperties
                {
                    ReplyMention = false
                },
                Embeds = new[] { embed }
            };

            await client.SendMessageAsync(message.ChannelId, embedWaifu);

        }

        private Character GetRandomCharacterFromDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Name, Series, Genre, Images FROM characters ORDER BY RANDOM() LIMIT 1";

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
}
