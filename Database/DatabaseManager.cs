using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace HutaoWaifuBot
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager(string dbPath)
        {
            var directory = Path.GetDirectoryName(dbPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _connectionString = $"Data Source={dbPath};";
            CreateDatabase();
        }

        public void CreateDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                // Characters table
                var createCharactersTableCmd = connection.CreateCommand();
                createCharactersTableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS characters (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Series TEXT NOT NULL,
                    Genre TEXT NOT NULL,
                    Images TEXT NOT NULL
                );";
                createCharactersTableCmd.ExecuteNonQuery();

                // Users table
                var createUsersTableCmd = connection.CreateCommand();
                createUsersTableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS users (
                    UserID TEXT NOT NULL,
                    ServerID TEXT NOT NULL,
                    CharactersOwned TEXT NOT NULL
                );";
                createUsersTableCmd.ExecuteNonQuery();
            }
        }
        public void AddCharactersInBulk(List<Character> characters)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            foreach (var character in characters)
            {
                string insertCharacter = @"
        INSERT INTO characters (Name, Series, Genre, Images)
        VALUES (@Name, @Series, @Genre, @Images);";

                using var command = new SqliteCommand(insertCharacter, connection);
                command.Parameters.AddWithValue("@Name", character.Name);
                command.Parameters.AddWithValue("@Series", character.Series);
                command.Parameters.AddWithValue("@Genre", character.Gender);
                command.Parameters.AddWithValue("@Images", character.Images);

                // Associa a transação ao comando
                command.Transaction = transaction;

                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }


        public void AddCharacter(string name, string series, string gender, string images)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"
                INSERT INTO characters (Name, Series, Genre, Images)
                VALUES ($name, $series, $gender, $images);";

                insertCommand.Parameters.AddWithValue("$name", name);
                insertCommand.Parameters.AddWithValue("$series", series);
                insertCommand.Parameters.AddWithValue("$gender", gender);
                insertCommand.Parameters.AddWithValue("$images", images);

                insertCommand.ExecuteNonQuery();
            }
        }

        public void AddUser(string userId, string serverId, string charactersOwned)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"
                INSERT INTO users (UserID, ServerID, CharactersOwned)
                VALUES ($userId, $serverId, $charactersOwned);";

                insertCommand.Parameters.AddWithValue("$userId", userId);
                insertCommand.Parameters.AddWithValue("$serverId", serverId);
                insertCommand.Parameters.AddWithValue("$charactersOwned", charactersOwned);

                insertCommand.ExecuteNonQuery();
            }
        }
    }
}
