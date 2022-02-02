
using Callender.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace Callender.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> opt) : base(opt)
        {

        }
        public DbSet<Carrier> Carrier { get; set; }
        public DbSet<Premium> Premium { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Suggest> Suggest { get; set; }
        public DbSet<SuggestCarrier> SuggestCarriers { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCarrier> UserCarrier { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
    }
}
