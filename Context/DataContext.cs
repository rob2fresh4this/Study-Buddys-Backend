using Microsoft.EntityFrameworkCore;
using Study_Buddys_Backend.Models;

namespace Study_Buddys_Backend.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<UserModels> Users { get; set; }
        public DbSet<CommunityModel> Communitys { get; set; }
    }
}