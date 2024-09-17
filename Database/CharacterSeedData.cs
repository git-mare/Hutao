using HutaoWaifuBot;
using System.Collections.Generic;

namespace HutaoWaifuBot
{

    public static class CharacterSeedData
    {
        public static List<Character> GetCharacters()
        {
            return new List<Character>
            {
            // Tokyo Necro
            new Character { Name = "Ethica Kibanohara", Gender = "F", Series = "Tokyo Necro", Images = "https://i.imgur.com/r58O1n6.png" },
            new Character { Name = "Gijou Mitsumi", Gender = "F", Series = "Tokyo Necro", Images = "https://i.imgur.com/TNV9I8Z.png" },
            new Character { Name = "Kazuma Nagisa", Gender = "F", Series = "Tokyo Necro", Images = "https://i.imgur.com/R8GCD8p.png" },
            new Character { Name = "Ilia Hougyou", Gender = "F", Series = "Tokyo Necro", Images = "https://i.imgur.com/POviZJZ.png" },
            new Character { Name = "Con Su", Gender = "F", Series = "Tokyo Necro", Images = "https://i.imgur.com/1PoFrTR.png" },
             };
        }

        public static void SeedCharacters(DatabaseManager dbManager)
        {
            var characters = GetCharacters();
            dbManager.AddCharactersInBulk(characters);
        }

    }
}
