using Microsoft.EntityFrameworkCore;

namespace WebApplication8.Data
{
    public class GiftOfGiversContext:DbContext
    {
        public GiftOfGiversContext(DbContextOptions<GiftOfGiversContext> options) : base(options)
        {
        }

        public DbSet<WebApplication8.Models.User> Users { get; set; }
        public DbSet<WebApplication8.Models.Volunteer> Volunteers { get; set; }
        public DbSet<WebApplication8.Models.Resource> Resources { get; set; }
        public DbSet<WebApplication8.Models.Assignment> Assignments { get; set; }
        public DbSet<WebApplication8.Models.Disaster> Disasters { get; set; }
        public DbSet<WebApplication8.Models.Donation> Donations { get; set; }
        public DbSet<WebApplication8.Models.ReliefProject> ReliefProjects { get; set; }
    }
}
