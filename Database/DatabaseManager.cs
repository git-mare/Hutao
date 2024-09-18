using Microsoft.EntityFrameworkCore;

namespace HutaoWaifuBot.Database
{
    public class WaifuBotContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }

        public string DbPath { get; }

        public WaifuBotContext(DbContextOptions<WaifuBotContext> options) : base(options)
        {
            DbPath = "hutaowaifubot.db";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>()
                .HasKey(c => c.ID);

            modelBuilder.Entity<User>()
                .HasKey(u => new { u.UserID, u.ServerID });

            base.OnModelCreating(modelBuilder);
        }
    }

    public class Character
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Series { get; set; }
        public string Genre { get; set; }
        public string Images { get; set; }
    }

    public class User
    {
        public string UserID { get; set; }
        public string ServerID { get; set; }
        public string CharactersOwned { get; set; }
    }
}
