using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context
{
    public partial class WorldRankDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        public WorldRankDbContext(DbContextOptions<WorldRankDbContext> options)
        {

        }


    }
}
