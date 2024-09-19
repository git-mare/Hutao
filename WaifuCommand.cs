using static HutaoWaifuBot.WaifuCommand.Series;
using NetCord.Services.ApplicationCommands;
using System.Collections.Immutable;
using NetCord.Rest;
using System.Text;
using NetCord;
namespace HutaoWaifuBot;

public class WaifuCommand : ApplicationCommandModule<SlashCommandContext>
{
	private static readonly CompositeFormat ImgurFormat = CompositeFormat.Parse("https://i.imgur.com/{0}.png");
	private static readonly CompositeFormat DescriptionFormat = CompositeFormat.Parse("{0} • {1}");
	private static readonly Random RNG = new();

	private static readonly ImmutableArray<(string, Series, string)> FemaleCharacters =
	[
		// Tokyo Necro
		("Ethica Kibanohara", Necro,     "r58O1n6"), ("Gijou Mitsumi",     Necro,     "TNV9I8Z"), ("Kazuma Nagisa",     Necro,     "R8GCD8p"),
		("Ilia Hougyou",      Necro,     "POviZJZ"), ("Con Su",            Necro,     "1PoFrTR"), ("Kiriri Aso",        Necro,     "G0WSpXQ"),
		("Olga",              Necro,     "7xSmWij"), ("Ryouko Karasuzumi", Necro,     "QZepXzm"), ("Sophia Kawahara",   Necro,     "RKAWeAg"),

		// Solo Leveling
		("Cha Hae-In",        Solo,      "PZjkdln"), ("Choi Yoo-Ra",       Solo,      "i4cHgQG"), ("Esil Radiru",       Solo,      "wqclMtt"),
		("Han Song-I",        Solo,      "TF4qdbl"), ("Lee Ju-Hee",        Solo,      "2RE4OMm"), ("Lola",              Solo,      "lpuqE98"),
		("Sung Jin-Ah",       Solo,      "JACIblp"), ("Mari Ishida",       Solo,      "PoIXFdo"), ("Kanae Tawata",      Solo,      "w8zzoU6"),
		("Akari Shimizu",     Solo,      "9ouKaMe"), ("Yoo Soo-Hyun",      Solo,      "B3yNYwe"), ("Gina",              Solo,      "tGyw8Np"),
		("Female Mage",       Solo,      "p1usdKc"),

		// Darwin's Game
		("Karino Shuka",      Darwins,   "BBKVb6N"), ("Sui",               Darwins,   "SDn9dGA"), ("Liu Xuelan",        Darwins,   "0TaTF2p"),
		("Xiao",              Darwins,   "cmjoxqi"), ("Maria Anderson",    Darwins,   "F7R4kbg"), ("Sakamoto Kasumi",   Darwins,   "iLkcMWw"),
		("Norie",             Darwins,   "rV0OJHi"), ("Saigou Arisa",      Darwins,   "BBWLi5R"), ("Themis",            Darwins,   "BWafHl5"),
		("Hiiragi Suzune",    Darwins,   "eLXDlOh"), ("Kashiwagi Rein",    Darwins,   "dazBmcr"),

		// In/Spectre
		("Rikka Sakuragawa",  InSpectre, "cOs8yio"), ("Saki Yumihara",     InSpectre, "52AKgNP"), ("Karin Nanase",      InSpectre, "x4FxdM1"),
		("Hatsumi Nanase",    InSpectre, "NgaOVd1"), ("Kotoko Iwanaga",    InSpectre, "dcRsHSG"),

		// Honkai Impact 3rd
		("Yae Sakura",        HI3rd,     "2YwflIw"), ("Thersa Apocalypse", HI3rd,     "JbaPe2S"), ("Seele Vollerei",    HI3rd,     "e0k8N9X"),
		("Raiden Mei",        HI3rd,     "XYJmEUQ"), ("Rozaliya Olenyeva", HI3rd,     "xh6gmHR")
	];


	[SlashCommand("random", "rolls for a random waifu")]
	public async Task WaifuAsync()
	{
		(string Name, Series Series, string ImageCode) = FemaleCharacters[RNG.Next(FemaleCharacters.Length)];

		await RespondAsync(InteractionCallback.Message(new()
		{
			Components = [(new ActionRowProperties([(new ButtonProperties("Claim", "Claim", new("❤️"), ButtonStyle.Secondary))]))],
			AllowedMentions = new() { ReplyMention = false },
			Embeds = [(new EmbedProperties()
			{
				Title = Name,
				Description = string.Format(null, DescriptionFormat, "♀️", Series switch 
				{
					Necro => "Tokyo Necro",
					Solo => "Solo Leveling",
					Darwins => "Darwin's Game",
					InSpectre => "In/Spectre",
					HI3rd => "Honkai Impact 3rd",
					_ => "Unknown"
				}),
				Image = string.Format(null, ImgurFormat, ImageCode),
				Color = new(0xF7F0F0)
			})]
		}));
	}

	public enum Series
	{
		Necro,
		Solo,
		Darwins,
		InSpectre,
		HI3rd
	}
}
